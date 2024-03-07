using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Inventory;
using Arcy.Management;

namespace Arcy.Quests
{
	public class QuestManager : MonoBehaviour
	{
		[Header("Config")]
		[SerializeField] private bool loadQuestState = true;

		private Dictionary<string, Quest> _questMap;

		// Quest start requirements:
		private int _currentPlayerLevel;

		private void Awake()
		{
			CreateQuestMap();
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
				if (quest.state == QuestState.IN_PROGRESS)
				{
					quest.InstantiateCurrentQuestObjective(this.transform);
				}

				// broadcast the initial state of all quests on startup
				GameEventManager.instance.questEvents.QuestStateChange(quest);
			}
		}

		private void ChangeQuestState(string id, QuestState state)
		{
			Quest quest = GetQuestById(id);
			quest.state = state;
			GameEventManager.instance.questEvents.QuestStateChange(quest);
		}

		private bool CheckRequirementMet(Quest quest)
		{
			// Start true and require to be false
			bool meetsRequirement = true;

			// Check player level requirement
			if (_currentPlayerLevel < quest.info.levelRequirement)
				meetsRequirement = false;

			// check quest prerequisites for completion
			foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
			{
				if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
				{
					meetsRequirement = false;
					break;
				}
			}

			return meetsRequirement;
		}

		private void Update()
		{
			// Loop Through all the quests
			foreach (Quest quest in _questMap.Values)
			{
				// if we're now meeting the requirements, switch over to the CAN_START state
				if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementMet(quest))
				{
					ChangeQuestState(quest.info.id, QuestState.CAN_START);
				}
			}
		}

		private void StartQuest(string id)
		{
			Quest quest = GetQuestById(id);
			quest.InstantiateCurrentQuestObjective(this.transform);
			ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
		}

		private void AdvanceQuest(string id)
		{
			Quest quest = GetQuestById(id);

			// move on to the next objective
			quest.MoveToNextObjective();

			// if there are more objectives, instantiate the next one
			if (quest.CurrentObjectiveExists())
			{
				quest.InstantiateCurrentQuestObjective(this.transform);
			}
			else
			{
				ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
			}
		}

		private void FinishQuest(string id)
		{
			Quest quest = GetQuestById(id);
			ClaimRewards(quest);
			ChangeQuestState(quest.info.id, QuestState.FINISHED);
		}

		// Claim the rewards after finishing a quest.
		private void ClaimRewards(Quest quest)
		{

			// GameEventManager.instance.goldEvents.GoldGoldGained(quest.info.goldReward);
			// GameEventManager.instance.playerEvents.ExperienceGained(quest.info.experienceReward);
		}

		private void QuestObjectiveStateChange(string id, int objectiveIndex, QuestObjectiveState questObjectiveState)
		{
			Quest quest = GetQuestById(id);
			quest.StoreQuestStepState(questObjectiveState, objectiveIndex);
			ChangeQuestState(id, quest.state);
		}

		private Dictionary<string, Quest> CreateQuestMap()
		{
			// Load all QuestInfo Scriptable Objects in the Assets/Resources/Quests folder
			QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

			// Create the quest map
			Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

			foreach (QuestInfoSO questInfo in allQuests)
			{
				if (idToQuestMap.ContainsKey(questInfo.id))
				{
					Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
				}
				idToQuestMap.Add(questInfo.id, LoadQuest(questInfo));
			}

			return idToQuestMap;
		}

		private Quest GetQuestById(string id)
		{
			Quest quest = _questMap[id];
			if (quest == null)
			{
				Debug.LogError("ID not found in the Quest Map: " + id);
			}
			return quest;
		}

		// TODO - Change so that the changes saves at convenient intervals, rather than when quitting the game
		private void OnApplicationQuit()
		{
			foreach (Quest quest in _questMap.Values)
			{
				SaveQuest(quest);
			}
		}

		// TODO - Switch to the implemented saving system
		private void SaveQuest(Quest quest)
		{
			try
			{
				QuestData questData = quest.GetQuestData();
				// serialize using JsonUtility, but use whatever you want here (like JSON.NET)
				string serializedData = JsonUtility.ToJson(questData);
				// PlayerPrefs.SetString(quest.info.id, serializedData);

				Debug.Log(serializedData);
			}
			catch (System.Exception e)
			{
				Debug.LogError("Failed to save quest with id " + quest.info.id + ": " + e);
			}
		}

		// TODO - Switch to the implemented saving system
		private Quest LoadQuest(QuestInfoSO questInfo)
		{
			Quest quest = null;
			try
			{
				// load quest from saved data
				if (PlayerPrefs.HasKey(questInfo.id) && loadQuestState)
				{
					string serializedData = PlayerPrefs.GetString(questInfo.id);
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
				Debug.LogError("Failed to load quest with id " + quest.info.id + ": " + e);
			}
			return quest;
		}
	}
}
