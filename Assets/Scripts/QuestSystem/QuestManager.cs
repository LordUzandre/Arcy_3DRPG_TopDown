using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Management;

namespace Arcy.Quests
{
	public class QuestManager : MonoBehaviour
	{
		[Header("Config")]
		[Tooltip("Should QuestManager load up quests from SaveData (old version)?")]
		[SerializeField] private bool loadQuestState = true;

		public Dictionary<string, Quest> questLog; // key = string, value = Quest

		public Quest GetQuestByGuid(string questID)
		{
			Quest quest = questLog[questID];

			if (quest == null)
			{
				Debug.LogError("ID not found in the Quest Map: " + questID);
			}

			return quest;
		}

		private void Awake()
		{
			questLog = CreateQuestLog();
		}

		private void Start()
		{
			foreach (Quest quest in questLog.Values)
			{
				if (quest.currentStatusEnum == QuestStateEnum.STARTED)
				{
					Debug.Log($"QuestManager: {quest.questSO.guid} is ongoing");
					quest.InstantiateCurrentQuestObjective(transform);
				}

				// broadcast the initial state of all quests on startup
				GameEventManager.instance.questEvents.QuestStateChange(quest);
			}
		}

		private void OnEnable()
		{
			GameEventManager.instance.questEvents.onStartQuest += StartQuest;
			GameEventManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
			GameEventManager.instance.questEvents.onFinishQuest += FinishQuest;
			GameEventManager.instance.questEvents.onQuestObjectiveStateChange += QuestObjectiveStateChange;

			GameEventManager.instance.dialogueEvents.onDialogueFinished += DialogueFinished;
		}

		private void OnDisable()
		{
			GameEventManager.instance.questEvents.onStartQuest -= StartQuest;
			GameEventManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
			GameEventManager.instance.questEvents.onFinishQuest -= FinishQuest;
			GameEventManager.instance.questEvents.onQuestObjectiveStateChange -= QuestObjectiveStateChange;

			GameEventManager.instance.dialogueEvents.onDialogueFinished -= DialogueFinished;
		}

		// create quest map during Awake()
		private Dictionary<string, Quest> CreateQuestLog()
		{
			// Load all QuestInfo Scriptable Objects in the Assets/Resources/Quests folder
			QuestSO[] allQuests = Resources.LoadAll<QuestSO>("Quests");
			//QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

			// Create the quest map
			Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

			foreach (QuestSO questInfo in allQuests)
			{
				if (idToQuestMap.ContainsKey(questInfo.guid))
				{
					Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.guid);
				}
				else
				{
					// idToQuestMap.Add(questInfo.guid, LoadQuest(questInfo));
				}
			}

			return idToQuestMap;
		}

		#region events that affect the progress of objectives

		private void DialogueFinished(string speakerID)
		{
			foreach (Quest quest in questLog.Values)
			{
				// check non-started quests firsts
				// if we're now meeting the requirements, switch over to the CAN_START state
				if (quest.currentStatusEnum == QuestStateEnum.REQUIREMENTS_NOT_MET && CheckRequirementMet(quest))
				{
					ChangeQuestState(quest.questSO.guid, QuestStateEnum.CAN_START);
					return;
				}

				if (quest.currentStatusEnum == QuestStateEnum.STARTED)
				{
					// TODO:
					// check whether any of the ongoing quests are currently listening for this conversation.
				}
			}
		}
		#endregion

		private bool CheckRequirementMet(Quest quest)
		{
			// Start true and require to be set to false
			bool meetsRequirement = true;

			// check quest prerequisites for completion
			foreach (QuestInfoSO prerequisiteQuestInfo in quest.questSO.questPrerequisites)
			{
				if (GetQuestByGuid(prerequisiteQuestInfo.guid).currentStatusEnum != QuestStateEnum.FINISHED)
				{
					meetsRequirement = false;
					break;
				}
			}

			return meetsRequirement;
		}

		// called from questPoint (TODO - fix!)
		private void StartQuest(string questID)
		{
			Quest quest = GetQuestByGuid(questID);

			quest.InstantiateCurrentQuestObjective(transform);
			ChangeQuestState(quest.questSO.guid, QuestStateEnum.STARTED);
		}

		private void ChangeQuestState(string questID, QuestStateEnum state)
		{
			Quest quest = GetQuestByGuid(questID);

			quest.currentStatusEnum = state;
			GameEventManager.instance.questEvents.QuestStateChange(quest);
		}

		private void AdvanceQuest(string questID)
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
				ChangeQuestState(quest.questSO.guid, QuestStateEnum.CAN_FINISH);
			}
		}

		private void FinishQuest(string questID)
		{
			Quest quest = GetQuestByGuid(questID);

			ClaimRewards(quest);
			ChangeQuestState(quest.questSO.guid, QuestStateEnum.FINISHED);
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

		private void QuestObjectiveStateChange(string questID, int objectiveIndex, QuestObjectiveState questObjectiveState)
		{
			Quest quest = GetQuestByGuid(questID);
			quest.StoreQuestObjectiveStatus(questObjectiveState, objectiveIndex);
			ChangeQuestState(questID, quest.currentStatusEnum);
		}

		#region Save/Load

		// TODO - Change so that the changes saves at convenient intervals, rather than when quitting the game
		// private void OnApplicationQuit()
		// {
		// 	foreach (Quest quest in _questMap.Values)
		// 	{
		// 		SaveQuest(quest);
		// 	}
		// }

		// TODO - Switch to the implemented saving system
		private void SaveQuest(Quest quest)
		{
			try
			{
				QuestData questData = quest.GetQuestData();

				// serialize using JsonUtility, but use whatever you want here (like JSON.NET)
				string serializedData = JsonUtility.ToJson(questData);
				// PlayerPrefs.SetString(quest.info.guid, serializedData);

				Debug.Log(serializedData);
			}
			catch (System.Exception e)
			{
				Debug.LogError("Failed to save quest with id " + quest.questSO.guid + ": " + e);
			}
		}

		// TODO - Switch to the implemented saving system
		private Quest LoadQuest(QuestInfoSO questInfo)
		{
			Quest quest = null;
			try
			{
				// load quest from saved data
				if (PlayerPrefs.HasKey(questInfo.guid) && loadQuestState)
				{
					string serializedData = PlayerPrefs.GetString(questInfo.guid);
					QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
					quest = new Quest(questInfo, questData.state, questData.questObjectiveIndex, questData.questObjectiveStates);
				}
				// otherwise, initialize a new quest
				else
				{
					quest = new Quest(questInfo);
				}
			}
			catch (System.Exception e)
			{
				Debug.LogError("Failed to load quest with id " + quest.questSO.guid + ": " + e);
			}
			return quest;
		}

		#endregion
	}
}
