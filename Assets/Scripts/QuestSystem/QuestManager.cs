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
		[SerializeField] private bool loadQuestState = true;

		private Dictionary<string, Quest> _questMap;

		[SerializeField] private List<Quest> _ongoingQuests;

		// Quest requirements:
		// private int _currentPlayerLevel;

		private void Awake()
		{
			_questMap = CreateQuestMap();
		}

		private void OnEnable()
		{
			GameEventManager.instance.questEvents.onStartQuest += StartQuest;
			GameEventManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
			GameEventManager.instance.questEvents.onFinishQuest += FinishQuest;

			GameEventManager.instance.questEvents.onQuestObjectiveStateChange += QuestObjectiveStateChange;
		}

		private void OnDisable()
		{
			GameEventManager.instance.questEvents.onStartQuest -= StartQuest;
			GameEventManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
			GameEventManager.instance.questEvents.onFinishQuest -= FinishQuest;

			GameEventManager.instance.questEvents.onQuestObjectiveStateChange -= QuestObjectiveStateChange;
		}

		private void Start()
		{
			foreach (Quest quest in _questMap.Values)
			{
				if (quest.statusEnum == QuestStateEnum.STARTED)
				{
					Debug.Log($"QuestManager: {quest.infoSO.guid} is ongoing");
					quest.InstantiateCurrentQuestObjective(this.transform);
				}

				// broadcast the initial state of all quests on startup
				GameEventManager.instance.questEvents.QuestStateChange(quest);
			}
		}

		// create quest map during Awake()
		private Dictionary<string, Quest> CreateQuestMap()
		{
			// Load all QuestInfo Scriptable Objects in the Assets/Resources/Quests folder
			QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

			// Create the quest map
			Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

			foreach (QuestInfoSO questInfo in allQuests)
			{
				if (idToQuestMap.ContainsKey(questInfo.guid))
				{
					Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.guid);
				}
				else
				{
					idToQuestMap.Add(questInfo.guid, LoadQuest(questInfo));
				}
			}

			return idToQuestMap;
		}

		// Run during update
		private bool CheckRequirementMet(Quest quest)
		{
			// Start true and require to be set to false
			bool meetsRequirement = true;

			// check quest prerequisites for completion
			foreach (QuestInfoSO prerequisiteQuestInfo in quest.infoSO.questPrerequisites)
			{
				if (GetQuestByGuid(prerequisiteQuestInfo.guid).statusEnum != QuestStateEnum.FINISHED)
				{
					meetsRequirement = false;
					break;
				}
			}

			// TODO - Check item in inventory requirement

			// TODO - Check dialogue requirement

			return meetsRequirement;
		}

		private void Update()
		{
			// TODO - subscribe to appropriate events rather than check every frame

			// Loop Through all the quests
			foreach (Quest quest in _questMap.Values)
			{
				// if we're now meeting the requirements, switch over to the CAN_START state
				if (quest.statusEnum == QuestStateEnum.REQUIREMENTS_NOT_MET && CheckRequirementMet(quest))
				{
					ChangeQuestState(quest.infoSO.guid, QuestStateEnum.CAN_START);
				}
			}
		}

		// called from questPoint (TODO - fix!)
		private void StartQuest(string id)
		{
			Quest quest = GetQuestByGuid(id);
			quest.InstantiateCurrentQuestObjective(this.transform);
			ChangeQuestState(quest.infoSO.guid, QuestStateEnum.STARTED);
		}

		private void ChangeQuestState(string id, QuestStateEnum state)
		{
			Quest quest = GetQuestByGuid(id);
			quest.statusEnum = state;
			GameEventManager.instance.questEvents.QuestStateChange(quest);
		}

		private void AdvanceQuest(string id)
		{
			Quest quest = GetQuestByGuid(id);

			// move on to the next objective
			quest.AdvanceToNextObjective();

			// if there are more objectives, instantiate the next one
			if (quest.CurrentObjectiveExists())
			{
				quest.InstantiateCurrentQuestObjective(this.transform);
			}
			else
			{
				ChangeQuestState(quest.infoSO.guid, QuestStateEnum.CAN_FINISH);
			}
		}

		private void FinishQuest(string id)
		{
			Quest quest = GetQuestByGuid(id);
			ClaimRewards(quest);
			ChangeQuestState(quest.infoSO.guid, QuestStateEnum.FINISHED);
		}

		// Claim the rewards after finishing a quest.
		private void ClaimRewards(Quest quest)
		{
			foreach (Inventory.InventorySlot item in quest.infoSO.rewardItems)
			{
				// Add items to inventory
			}
		}

		private void QuestObjectiveStateChange(string id, int objectiveIndex, QuestObjectiveState questObjectiveState)
		{
			Quest quest = GetQuestByGuid(id);
			quest.StoreQuestObjectiveStatus(questObjectiveState, objectiveIndex);
			ChangeQuestState(id, quest.statusEnum);
		}

		#region questMap

		private Quest GetQuestByGuid(string id)
		{
			Quest quest = _questMap[id];
			if (quest == null)
			{
				Debug.LogError("ID not found in the Quest Map: " + id);
			}
			return quest;
		}

		#endregion

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
				Debug.LogError("Failed to save quest with id " + quest.infoSO.guid + ": " + e);
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
				Debug.LogError("Failed to load quest with id " + quest.infoSO.guid + ": " + e);
			}
			return quest;
		}

		#endregion
	}
}
