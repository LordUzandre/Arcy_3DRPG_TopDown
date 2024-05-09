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
		[SerializeField] private List<string> _allQuests = new List<string>(); // Used only in the editor

		private static Dictionary<int, Quest> _questCache; // int = Key, Quest = Value

		// MARK: PUBLIC:

		public Quest GetQuestByGuid(int questID)
		{
			// Create the quest map
			_questCache ??= CreateQuestCache();

			Quest quest = _questCache[questID];

			if (quest == null || !_questCache.ContainsKey(questID))
			{
				Debug.LogError("QuestManager: ID not found in the Quest Map: " + questID);
				return null;
			}

			return _questCache[questID];
		}

		//MARK: PRIVATE:

		private Dictionary<int, Quest> CreateQuestCache()
		{
			Dictionary<int, Quest> idToQuestMap = new Dictionary<int, Quest>();
			IEnumerable allQuests = Resources.LoadAll<QuestSO>("Quests");

			foreach (QuestSO questSO in allQuests)
			{
				if (idToQuestMap.ContainsKey(questSO.questGuid))
				{
					Debug.LogWarning("Duplicate ID found when creating quest map: " + questSO.questGuid);
				}

				idToQuestMap.Add(questSO.questGuid, SetupQuest(questSO));
			}

			if (_debugging) Debug.Log("QuestManager: Quest Log created");

			return idToQuestMap;
		}

		private void SetupQuestLog(SaveData loadData) // called by LoadData()
		{
			foreach (Quest quest in _questCache.Values)
			{
				if (_debugging) Debug.Log("Quest: '" + quest.QuestObject.questName + "' added to quest log and is '" + quest.CurrentStatusEnum + "'");

				// _allQuests.Add(quest.QuestObject.questName.ToString() + " = " + quest.CurrentStatusEnum);

				// if (quest.CurrentStatusEnum == QuestObjectiveEnum.STARTED)
				// {
				// 	if (_debugging) Debug.Log($"QuestManager: " + quest.QuestObject.questGuid + " is ongoing");

				// 	quest.InstantiateCurrentQuestObjective(transform);
				// }
			}
		}

		private Quest SetupQuest(QuestSO questSO) // used by CreateQuestCache()
		{
			Quest quest = null;

			try
			{
				// load quest from saved data
				if (saveData.questLog.ContainsKey(questSO.questGuid) && _useSaveData)
				{
					string serializedData = saveData.questLog[questSO.questGuid].ToString();
					QuestSaveData savedQuestData = JsonUtility.FromJson<QuestSaveData>(serializedData);
					quest = new Quest(questSO, savedQuestData.State, savedQuestData.QuestObjectiveIndex); //, questData.QuestObjectiveStates);
				}
				// otherwise, initialize a new quest
				else
				{
					quest = new Quest(questSO);
				}
			}
			catch (System.Exception e)
			{
				Debug.LogError("Failed to load quest with id " + quest.QuestObject.questGuid + ": " + e);
			}

			return quest;
		}

		private void OnEnable()
		{
			GameManager.instance.gameEventManager.questEvents.onStartQuest += StartQuest;
			GameManager.instance.gameEventManager.questEvents.onAdvanceQuest += AdvanceQuest;
			GameManager.instance.gameEventManager.questEvents.onFinishQuest += FinishQuest;

			// GameManager.instance.gameEventManager.questEvents.onQuestObjectiveStateChange += QuestObjectiveStateChange;
			// GameManager.instance.gameEventManager.dialogueEvents.onDialogueFinished += DialogueFinished;
			// GameManager.instance.gameEventManager.inventoryEvents.onInventoryUpdated += InventoryUpdated;
		}

		private void OnDisable()
		{
			GameManager.instance.gameEventManager.questEvents.onStartQuest -= StartQuest;
			GameManager.instance.gameEventManager.questEvents.onAdvanceQuest -= AdvanceQuest;
			GameManager.instance.gameEventManager.questEvents.onFinishQuest -= FinishQuest;

			// GameManager.instance.gameEventManager.questEvents.onQuestObjectiveStateChange -= QuestObjectiveStateChange;
			// GameManager.instance.gameEventManager.dialogueEvents.onDialogueFinished -= DialogueFinished;
			// GameManager.instance.gameEventManager.inventoryEvents.onInventoryUpdated -= InventoryUpdated;
		}

		// private void DialogueFinished(int speakerID)
		// {
		// 	foreach (Quest quest in _questCache.Values)
		// 	{
		// 		// check non-started quests firsts
		// 		// if we're now meeting the requirements, switch over to the CAN_START state
		// 		if (quest.CurrentStatusEnum == QuestObjectiveEnum.REQUIREMENTS_NOT_MET && CheckRequirementMet(quest))
		// 		{
		// 			ChangeQuestState(quest.QuestObject.questGuid, QuestObjectiveEnum.CAN_START);
		// 			return;
		// 		}

		// 		if (quest.CurrentStatusEnum == QuestObjectiveEnum.STARTED)
		// 		{
		// 			// TODO:
		// 			// check whether any of the ongoing quests are currently listening for this conversation.
		// 		}
		// 	}
		// }

		// private void InventoryUpdated()
		// {
		// 	Debug.Log("QuestManager is registering that you have picked up a new item.");
		// }

		private bool CheckRequirementMet(Quest quest)
		{
			// Start true and require to be set to false
			bool meetsRequirement = true;

			// check quest prerequisites for completion
			foreach (QuestRequirement requirement in quest.QuestObject.requirementsToStartQuest)
			{
				switch (requirement.requirementType)
				{
					case (RequirementEnum.ItemID): // Look through inventory to see if we have all that's needed.
						foreach (Inventory.InventorySlot slot in Inventory.InventoryManager.ConsumableSlots)
						{
							if (slot.GetItem().GetGuid() == requirement.itemID)
							{
								if (slot.GetAmount() <= requirement.requiredItemAmount)
								{
									meetsRequirement = false;
								}
							}
						}
						break;

					case (RequirementEnum.DialogueID): // See if we have talked to the character
						break;

					case (RequirementEnum.BattleID): // Check battlelog to see if we have fought the required character
						break;

					case (RequirementEnum.PlayerLevel): // Check to see if the player is at the required level
						break;

					case (RequirementEnum.TeamMember): // Check the battle party to see if we have the required party member
						break;

					case (RequirementEnum.PreviousQuestID): // Check the questLog to see if we have finished the required quests
						if (GetQuestByGuid(requirement.requiredPreviousQuestID).CurrentStatusEnum != QuestObjectiveEnum.FINISHED) return false;
						break;
				}
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

		private void QuestObjectiveStateChange(int questID, int objectiveIndex) //, QuestObjectiveState questObjectiveState)
		{
			Quest quest = GetQuestByGuid(questID);
			// quest.StoreQuestObjectiveStatus(questObjectiveState, objectiveIndex);
			ChangeQuestState(questID, quest.CurrentStatusEnum);
		}

		// MARK: Save/Load:

		public void LoadData(SaveData loadData)
		{

			// #if UNITY_EDITOR
			// 			if (!SaveDataManager.GlobalOverrideSaveData)
			// 			{
			// 				if (!_useSaveData)
			// 				{
			// 					return;
			// 				}
			// 			}
			// #endif

			if (loadData.questLog.Count > 0)
			{
				SetupQuestLog(loadData);
			}
		}

		public void SaveData(SaveData saveData)
		{

			// #if UNITY_EDITOR
			// 			if (!_useSaveData || !SaveDataManager.GlobalOverrideSaveData)
			// 			{
			// 				return;
			// 			}
			// #endif

			// 			foreach (Quest quest in _questCache.Values)
			// 			{
			// 				SaveQuestProgress(quest);
			// 			}
		}

		// Convert the questData into Json
		// private void SaveQuestProgress(Quest quest)
		// {
		// 	try
		// 	{
		// 		QuestSaveData questData = quest.GetQuestData();

		// 		string serializedData = JsonUtility.ToJson(questData);

		// 		// TODO: Add questStatus to SaveData.

		// 		if (_debugging) Debug.Log("QuestManager: " + serializedData);
		// 	}
		// 	catch (System.Exception e)
		// 	{
		// 		Debug.LogError("Failed to save quest with id " + quest.QuestObject.questGuid + ": " + e);
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
	}
}
