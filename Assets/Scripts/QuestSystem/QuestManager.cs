using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Management;
using Arcy.Saving;

namespace Arcy.Quests
{
	public class QuestManager : MonoBehaviour, ISaveableEntity
	{
		[Header("Config")]
		[SerializeField] private bool _debugging = false;
		[SerializeField] private bool _useSaveData;

		private static Dictionary<int, Quest> _questCache; // Key = string, Value = Quest


		// MARK: PUBLIC:

		public Quest GetQuestByGuid(int questID)
		{
			if (_questCache == null)
			{
				// Create the quest map
				Dictionary<int, Quest> idToQuestMap = new Dictionary<int, Quest>();

				// Load all Quest Scriptable Objects in the Assets/Resources/Quests folder
				IEnumerable allQuests = Resources.LoadAll<QuestSO>("Quests");

				foreach (QuestSO questSO in allQuests)
				{
					if (idToQuestMap.ContainsKey(questSO.questGuid))
					{
						Debug.LogWarning("Duplicate ID found when creating quest map: " + questSO.questGuid);
					}

					idToQuestMap.Add(questSO.questGuid, LoadQuest(questSO));
				}

				if (_debugging) Debug.Log("Quest Log created");
			}

			Quest quest = _questCache[questID];

			if (quest == null || !_questCache.ContainsKey(questID))
			{
				Debug.LogError("ID not found in the Quest Map: " + questID);
				return null;
			}

			return _questCache[questID];
		}

		//MARK: PRIVATE:

		private void Start()
		{
			foreach (Quest quest in _questCache.Values)
			{
				if (quest.CurrentStatusEnum == QuestObjectiveEnum.STARTED)
				{
					Debug.Log($"QuestManager: {quest.QuestObject.questGuid} is ongoing");
					quest.InstantiateCurrentQuestObjective(transform);
				}

				// broadcast the initial state of all quests on startup
				//GameManager.instance.gameEventManager.questEvents.StartQuest();
			}
		}

		private void OnEnable()
		{
			GameManager.instance.gameEventManager.questEvents.onStartQuest += StartQuest;
			GameManager.instance.gameEventManager.questEvents.onAdvanceQuest += AdvanceQuest;
			GameManager.instance.gameEventManager.questEvents.onFinishQuest += FinishQuest;
			// GameManager.instance.gameEventManager.questEvents.onQuestObjectiveStateChange += QuestObjectiveStateChange;

			GameManager.instance.gameEventManager.dialogueEvents.onDialogueFinished += DialogueFinished;
		}

		private void OnDisable()
		{
			GameManager.instance.gameEventManager.questEvents.onStartQuest -= StartQuest;
			GameManager.instance.gameEventManager.questEvents.onAdvanceQuest -= AdvanceQuest;
			GameManager.instance.gameEventManager.questEvents.onFinishQuest -= FinishQuest;
			// GameManager.instance.gameEventManager.questEvents.onQuestObjectiveStateChange -= QuestObjectiveStateChange;

			GameManager.instance.gameEventManager.dialogueEvents.onDialogueFinished -= DialogueFinished;
		}

		private void DialogueFinished(int speakerID)
		{
			foreach (Quest quest in _questCache.Values)
			{
				// check non-started quests firsts
				// if we're now meeting the requirements, switch over to the CAN_START state
				if (quest.CurrentStatusEnum == QuestObjectiveEnum.REQUIREMENTS_NOT_MET && CheckRequirementMet(quest))
				{
					ChangeQuestState(quest.QuestObject.questGuid, QuestObjectiveEnum.CAN_START);
					return;
				}

				if (quest.CurrentStatusEnum == QuestObjectiveEnum.STARTED)
				{
					// TODO:
					// check whether any of the ongoing quests are currently listening for this conversation.
				}
			}
		}

		private bool CheckRequirementMet(Quest quest)
		{
			// Start true and require to be set to false
			bool meetsRequirement = true;

			// check quest prerequisites for completion
			foreach (QuestRequirement requiredFinishedQuest in quest.QuestObject.requirementsToStartQuest)
			{
				// if (GetQuestByGuid(requiredFinishedQuest.questGuid).CurrentStatusEnum != QuestObjectiveEnum.FINISHED)
				// {
				// 	return false;
				// }
			}

			return meetsRequirement;
		}

		private void StartQuest(int questID)
		{
			Quest quest = GetQuestByGuid(questID);

			quest.InstantiateCurrentQuestObjective(transform);
			ChangeQuestState(quest.QuestObject.questGuid, QuestObjectiveEnum.STARTED);
		}

		private void ChangeQuestState(int questID, QuestObjectiveEnum state)
		{
			Quest quest = GetQuestByGuid(questID);

			quest.CurrentStatusEnum = state;
			// GameManager.instance.gameEventManager.questEvents.QuestStateChange(quest);
		}

		private void AdvanceQuest(int questID)
		{
			Quest quest = GetQuestByGuid(questID);

			// move on to the next objective
			quest.AdvanceToNextObjective();

			// if there are more objectives, instantiate the next one else put the quest in CAN_FINISH-mode
			if (quest.CurrentQuestObjectiveExists())
			{
				quest.InstantiateCurrentQuestObjective(transform);
			}
			else
			{
				ChangeQuestState(quest.QuestObject.questGuid, QuestObjectiveEnum.CAN_FINISH);
			}
		}

		private void FinishQuest(int questID)
		{
			Quest quest = GetQuestByGuid(questID);

			ClaimRewards(quest);
			ChangeQuestState(quest.QuestObject.questGuid, QuestObjectiveEnum.FINISHED);
		}

		// Claim the rewards after finishing a quest.
		private void ClaimRewards(Quest quest)
		{
			// TODO - Add items to inventory
			// Remember to alert UI
		}

		private void QuestObjectiveStateChange(int questID, int objectiveIndex, QuestObjectiveState questObjectiveState)
		{
			Quest quest = GetQuestByGuid(questID);
			quest.StoreQuestObjectiveStatus(questObjectiveState, objectiveIndex);
			ChangeQuestState(questID, quest.CurrentStatusEnum);
		}

		// MARK: Save/Load:

		public void LoadData(SaveData loadData)
		{

#if UNITY_EDITOR
			if (!_useSaveData)
			{
				return;
			}
#endif

			// TODO: Fill out the rest.

		}

		public void SaveData(SaveData saveData)
		{

#if UNITY_EDITOR
			if (!_useSaveData)
			{
				return;
			}
#endif

			foreach (Quest quest in _questCache.Values)
			{
				SaveQuestProgress(quest);
			}
		}

		// Convert the questData into Json
		private void SaveQuestProgress(Quest quest)
		{
			try
			{
				QuestSaveData questData = quest.GetQuestData();

				string serializedData = JsonUtility.ToJson(questData);

				// TODO: Add questStatus to SaveData.

				Debug.Log(serializedData);
			}
			catch (System.Exception e)
			{
				Debug.LogError("Failed to save quest with id " + quest.QuestObject.questGuid + ": " + e);
			}

		}

		// TODO - Change so that the changes saves at convenient intervals, rather than when quitting the game
		// private void OnApplicationQuit()
		// {
		// 	foreach (Quest quest in _questMap.Values)
		// 	{
		// 		SaveQuest(quest);
		// 	}
		// }

		// TODO - Switch to the implemented saving system
		// private void SaveQuest(Quest quest)
		// {
		// 	try
		// 	{
		// 		QuestSaveData questData = quest.GetQuestData();

		// 		// serialize using JsonUtility, but use whatever you want here (like JSON.NET)
		// 		string serializedData = JsonUtility.ToJson(questData);
		// 		// PlayerPrefs.SetString(quest.info.guid, serializedData);

		// 		Debug.Log(serializedData);
		// 	}
		// 	catch (System.Exception e)
		// 	{
		// 		Debug.LogError("Failed to save quest with id " + quest.questSO.guid + ": " + e);
		// 	}
		// }

		// // TODO - Switch to the implemented saving system
		// private Quest LoadQuest(QuestInfoSO questInfo)
		// {
		// 	Quest quest = null;
		// 	try
		// 	{
		// 		// load quest from saved data
		// 		if (PlayerPrefs.HasKey(questInfo.guid.ToString()) && loadQuestState)
		// 		{
		// 			string serializedData = PlayerPrefs.GetString(questInfo.guid.ToString());
		// 			QuestSaveData questData = JsonUtility.FromJson<QuestSaveData>(serializedData);
		// 			quest = new Quest(questInfo, questData.state, questData.questObjectiveIndex, questData.questObjectiveStates);
		// 		}
		// 		// otherwise, initialize a new quest
		// 		else
		// 		{
		// 			quest = new Quest(questInfo);
		// 		}
		// 	}
		// 	catch (System.Exception e)
		// 	{
		// 		Debug.LogError("Failed to load quest with id " + quest.questSO.guid + ": " + e);
		// 	}
		// 	return quest;
		// }

		private Quest LoadQuest(QuestSO questSO)
		{
			return null;
		}
	}
}
