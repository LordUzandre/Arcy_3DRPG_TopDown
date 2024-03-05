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
		}

		private void OnDisable()
		{
			GameEventManager.instance.questEvents.onStartQuest -= StartQuest;
			GameEventManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
			GameEventManager.instance.questEvents.onFinishQuest -= FinishQuest;
		}

		private void Start()
		{
			// broadcast the initial state of all quests on startup
			foreach (Quest quest in _questMap.Values)
			{
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

		private void ClaimRewards(Quest quest)
		{
			// GameEventManager.instance.goldEvents.GoldGoldGained(quest.info.goldReward);
			// GameEventManager.instance.playerEvents.ExperienceGained(quest.info.experienceReward);
		}

		private Dictionary<string, Quest> CreateQuestMap()
		{
			// Loads all QuestInfo Scriptable Objects under the Assets/Resources/Quests folder
			QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

			// Create the quest map
			Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

			foreach (QuestInfoSO questInfo in allQuests)
			{
				if (idToQuestMap.ContainsKey(questInfo.id))
				{
					Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
				}
				idToQuestMap.Add(questInfo.id, new Quest(questInfo));
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
	}
}
