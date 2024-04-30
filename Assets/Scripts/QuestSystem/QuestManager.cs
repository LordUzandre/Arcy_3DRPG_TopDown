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

		// [Tooltip("Should QuestManager load up quests from SaveData (old version)?")]
		// [SerializeField] private bool loadQuestState = true;

		private static Dictionary<int, Quest> questCache; // Key = string, Value = Quest

		[SerializeField] private QuestSO[] quests;
		// [SerializeField] private Dictionary<int, QuestSO> _questLog;

		// MARK: PUBLIC:

		public Quest GetQuestByGuid(int questID)
		{
			if (questCache == null)
			{
				// Create the quest map
				Dictionary<int, Quest> idToQuestMap = new Dictionary<int, Quest>();

				// Load all Quest Scriptable Objects in the Assets/Resources/Quests folder
				IEnumerable allQuests = Resources.LoadAll<QuestSO>("Quests");

				foreach (QuestSO questSO in allQuests)
				{
					if (idToQuestMap.ContainsKey(questSO.questGUID))
					{
						Debug.LogWarning("Duplicate ID found when creating quest map: " + questSO.questGUID);
					}

					idToQuestMap.Add(questSO.questGUID, LoadQuest(questSO));
				}
			}

			Quest quest = questCache[questID];

			if (quest == null || !questCache.ContainsKey(questID))
			{
				Debug.LogError("ID not found in the Quest Map: " + questID);
				return null;
			}

			return questCache[questID];
		}

		//MARK: PRIVATE:

		private void Start()
		{
			foreach (Quest quest in questCache.Values)
			{
				if (quest.currentStatusEnum == QuestObjectiveEnum.STARTED)
				{
					Debug.Log($"QuestManager: {quest.questSO.guid} is ongoing");
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

		// create quest map during Awake()
		// private static Dictionary<int, Quest> CreateQuestLog()
		// {
		// 	// Load all Quest Scriptable Objects in the Assets/Resources/Quests folder
		// 	QuestSO[] allQuests = Resources.LoadAll<QuestSO>("Quests");
		// 	// QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

		// 	// Create the quest map
		// 	Dictionary<int, Quest> idToQuestMap = new Dictionary<int, Quest>();

		// 	foreach (QuestSO questSO in allQuests)
		// 	{
		// 		if (idToQuestMap.ContainsKey(questSO.questGUID))
		// 		{
		// 			Debug.LogWarning("Duplicate ID found when creating quest map: " + questSO.questGUID);
		// 		}
		// 		else
		// 		{
		// 			// idToQuestMap.Add(questSO.questGUID, LoadQuest(questSO));
		// 		}
		// 	}

		// 	return idToQuestMap;
		// }

		private void DialogueFinished(int speakerID)
		{
			foreach (Quest quest in questCache.Values)
			{
				// check non-started quests firsts
				// if we're now meeting the requirements, switch over to the CAN_START state
				if (quest.currentStatusEnum == QuestObjectiveEnum.REQUIREMENTS_NOT_MET && CheckRequirementMet(quest))
				{
					ChangeQuestState(quest.questSO.guid, QuestObjectiveEnum.CAN_START);
					return;
				}

				if (quest.currentStatusEnum == QuestObjectiveEnum.STARTED)
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
			foreach (QuestInfoSO prerequisiteQuestInfo in quest.questSO.questPrerequisites)
			{
				if (GetQuestByGuid(prerequisiteQuestInfo.guid).currentStatusEnum != QuestObjectiveEnum.FINISHED)
				{
					meetsRequirement = false;
					break;
				}
			}

			return meetsRequirement;
		}

		private void StartQuest(int questID)
		{
			Quest quest = GetQuestByGuid(questID);

			quest.InstantiateCurrentQuestObjective(transform);
			ChangeQuestState(quest.questSO.guid, QuestObjectiveEnum.STARTED);
		}

		private void ChangeQuestState(int questID, QuestObjectiveEnum state)
		{
			Quest quest = GetQuestByGuid(questID);

			quest.currentStatusEnum = state;
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
				ChangeQuestState(quest.questSO.guid, QuestObjectiveEnum.CAN_FINISH);
			}
		}

		private void FinishQuest(int questID)
		{
			Quest quest = GetQuestByGuid(questID);

			ClaimRewards(quest);
			ChangeQuestState(quest.questSO.guid, QuestObjectiveEnum.FINISHED);
		}

		// Claim the rewards after finishing a quest.
		private void ClaimRewards(Quest quest)
		{
			foreach (Inventory.InventorySlot rewardItem in quest.questSO.rewardItems)
			{
				// TODO - Add items to inventory
				// Remember to alert UI
			}
		}

		private void QuestObjectiveStateChange(int questID, int objectiveIndex, QuestObjectiveState questObjectiveState)
		{
			Quest quest = GetQuestByGuid(questID);
			quest.StoreQuestObjectiveStatus(questObjectiveState, objectiveIndex);
			ChangeQuestState(questID, quest.currentStatusEnum);
		}

		// MARK: Save/Load:

		public void LoadData(SaveData loadData)
		{
		}

		public void SaveData(SaveData saveData)
		{
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
