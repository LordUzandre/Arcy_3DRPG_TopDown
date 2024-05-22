using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Management;
using Arcy.Saving;
using UnityEngine.InputSystem.Interactions;
using System.Linq;

namespace Arcy.Quests
{
	public class QuestManager : MonoBehaviour, ISaveableEntity
	{
		[Header("Config")]
		[SerializeField] private bool _debugging = false;
		[SerializeField] private bool _useSaveData;
		[Space]
		[SerializeField] private List<string> inEditorQuestList = new List<string>();

		private static Dictionary<int, Quest> _questCache; // int = Key, Quest = Value

		// MARK: PUBLIC:

		public Quest GetQuestByGuid(int questID)
		{
			Quest quest = _questCache[questID];

			if (quest == null || !_questCache.ContainsKey(questID))
			{
				Debug.LogError("QM: Quest " + questID + " could not be found in the questCache.");
				return null;
			}

			return quest;
		}

		//MARK: PRIVATE:

#if UNITY_EDITOR
		private void Awake()
		{
			if (!_useSaveData && _questCache == null)
			{
				_questCache = CreateQuestCache();
				if (_debugging) Debug.Log("QM: Creating QuestCache without using saveData");
			}
		}
#endif

		// EDITOR
		private List<string> UpdateEditorList()
		{
			List<string> newList = new List<string>();

			foreach (var quest in _questCache.Values)
			{
				string inEditorQuest = quest.QuestObject.questName + " = " + quest.CurrentStatusEnum.ToString();
				inEditorQuestList.Add(inEditorQuest);
			}

			return newList;
		}

		private Dictionary<int, Quest> CreateQuestCache()
		{
			Dictionary<int, Quest> idToQuestMap = new Dictionary<int, Quest>();

			// Load in all quests from folder
			IEnumerable allQuests = Resources.LoadAll<QuestSO>("Quests");

			foreach (QuestSO questSO in allQuests)
			{
				// if (_debugging) Debug.Log("QM: Quest " + questSO.questName + " added to questLog");

				// Check for duplicates
				if (idToQuestMap.ContainsKey(questSO.questGuid)) Debug.LogWarning("QM: Duplicate ID found when creating quest map: " + questSO.questGuid);

				idToQuestMap.Add(questSO.questGuid, new Quest(questSO));
			}

			return idToQuestMap;
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
			bool requirementMet = false;

			if (quest.QuestObject.requirementsToStartQuest.Length == 0)
			{
				return true;
			}

			// check quest prerequisites for completion
			foreach (QuestRequirement requirement in quest.QuestObject.requirementsToStartQuest)
			{
				switch (requirement.requirementType)
				{
					// Look through inventory to see if we have all that's needed.
					case (RequirementEnum.ItemID):

						if (Inventory.InventoryManager.ConsumableSlots == null)
						{
							Debug.LogError("QM: No Inventory Found");
							return false;
						}

						foreach (Inventory.InventorySlot slot in Inventory.InventoryManager.ConsumableSlots)
						{
							int inventoryGuid = slot.GetItem().GetGuid();

							if (inventoryGuid == requirement.requiredItem.GetGuid())
							{
								if (slot.GetAmount() >= requirement.requiredItemAmount) { requirementMet = true; }
								else { requirementMet = false; }
								break;
							}
						}
						break;

					// See if we have talked to the character
					case (RequirementEnum.DialogueID):
						break;

					// Check battlelog to see if we have fought the required character
					case (RequirementEnum.BattleID):
						break;

					// Check to see if the player is at the required level
					case (RequirementEnum.PlayerLevel):
						break;

					// Check the battle party to see if we have the required party member
					case (RequirementEnum.TeamMember):
						break;

					// Check the questLog to see if we have finished the required quests
					case (RequirementEnum.PreviousQuestID):
						if (GetQuestByGuid(requirement.requiredPreviousQuestID).CurrentStatusEnum == QuestObjectiveEnum.FINISHED)
						{
							// if (_debugging) Debug.Log("QM: " + quest.QuestObject.questName + " can't be started until quest " + GetQuestByGuid(requirement.requiredPreviousQuestID) + " has been finished");
							requirementMet = true;
						}
						break;
				}
			}

			return requirementMet;
		}

		private void StartQuest(int questKey)
		{
			Quest quest = GetQuestByGuid(questKey);

			quest.InstantiateCurrentQuestObjective(transform);
			ChangeQuestState(quest.QuestObject.questGuid, QuestObjectiveEnum.STARTED);
		}

		private void ChangeQuestState(int questKey, QuestObjectiveEnum state)
		{
			Quest quest = GetQuestByGuid(questKey);

			quest.CurrentStatusEnum = state;
			// GameManager.instance.gameEventManager.questEvents.QuestStateChange(quest);
		}

		private void AdvanceQuest(int questKey)
		{
			Quest quest = GetQuestByGuid(questKey);

			// move on to the next objective
			quest.AdvanceToNextObjective();

			// if there are more objectives, instantiate the next one else put the quest in CAN_FINISH-mode
			if (quest.CurrentQuestObjectiveExists())
			{
				quest.InstantiateCurrentQuestObjective(transform);
			}
			else
			{
				//ChangeQuestState(quest.QuestObject.questGuid, QuestObjectiveEnum.CAN_FINISH);
				FinishQuest(questKey);
			}
		}

		private void FinishQuest(int questKey)
		{
			Quest quest = GetQuestByGuid(questKey);

			ClaimRewards(questKey);
			ChangeQuestState(quest.QuestObject.questGuid, QuestObjectiveEnum.FINISHED);
		}

		// Claim the rewards after finishing a quest.
		private void ClaimRewards(int questKey)
		{
			// TODO - Add items to inventory
			// Remember to alert UI
		}

		private void QuestObjectiveStateChange(int questID, int objectiveIndex, QuestObjectiveState questObjectiveState)
		{
			Quest quest = GetQuestByGuid(questID);
			quest.StoreQuestObjectiveStatus(questObjectiveState, objectiveIndex);
			// ChangeQuestState(questID, quest.CurrentStatusEnum);
		}

		// MARK: Save/Load:

		public void LoadData(SaveData loadData)
		{

#if UNITY_EDITOR
			if (!_useSaveData)
			{
				Debug.Log("Not using SaveData.");
				return;
			}
#endif

			if (_debugging) Debug.Log("QM: Setting up quest log using SaveData");

			if (loadData.questLog.Count >= 0) // Convert the status of all quests according to SaveData
			{
				_questCache ??= CreateQuestCache();

				StartCoroutine(SetupQuestLog());

			}

			IEnumerator SetupQuestLog()
			{
				yield return null;

				foreach (int guid in _questCache.Keys)
				{
					Quest quest = GetQuestByGuid(guid);

					if (_debugging) Debug.Log("Quest: " + quest.QuestObject.questName + " found in saveData");

					try
					{
						// load quest from saved data
						if (loadData.questLog.ContainsKey(guid))
						{
							string serializedData = loadData.questLog[guid].ToString();
							QuestSaveData savedQuestData = JsonUtility.FromJson<QuestSaveData>(serializedData);

							// quest = new Quest(quest.QuestObject, savedQuestData.State, savedQuestData.QuestObjectiveIndex); //, questData.QuestObjectiveStates);
							quest.CurrentStatusEnum = savedQuestData.State;

							switch (quest.CurrentStatusEnum)
							{
								case QuestObjectiveEnum.REQUIREMENTS_NOT_MET:
									// Check requirements
									if (CheckRequirementMet(quest))
									{
										// Start Quest
										// ChangeQuestState(guid, QuestObjectiveEnum.STARTED);
										Debug.Log("QM: " + quest.QuestObject.questName + " has started");
									}
									break;
								case QuestObjectiveEnum.CAN_START:
									// ??
									break;
								case QuestObjectiveEnum.STARTED:
									// Set objectiveIndex to correct index
									quest.currentQuestObjectiveIndex = savedQuestData.ObjectiveIndex;
									// 	quest.InstantiateCurrentQuestObjective(transform);
									// Show in UI
									break;
								case QuestObjectiveEnum.CAN_FINISH:
									// ??
									break;
								case QuestObjectiveEnum.FINISHED:
									// Only Show in UI
									break;
							}
						}
					}
					catch (System.Exception e)
					{
						Debug.LogError("Failed to load quest with id " + quest.QuestObject.questGuid + ": " + e);
					}

					yield return null;
				}

				inEditorQuestList = UpdateEditorList();
				yield return null;
			}
		}

		public void SaveData(SaveData saveData)
		{

#if UNITY_EDITOR
			if (!_useSaveData) { return; }
#endif

			if (_questCache == null)
			{
				Debug.Log("No quests to be saved");
				return;
			}

			foreach (Quest quest in _questCache.Values)
			{
				try
				{
					QuestSaveData questData = quest.GetQuestData();
					string serializedData = JsonUtility.ToJson(questData);

					if (saveData.questLog.ContainsKey(quest.QuestObject.questGuid))
					{
						saveData.questLog.Remove(quest.QuestObject.questGuid);
					}

					saveData.questLog.Add(quest.QuestObject.questGuid, serializedData);
				}
				catch (System.Exception e)
				{
					Debug.LogError("Failed to save quest " + quest.QuestObject.questName + ": " + e);
				}
			}
		}

	}
}
