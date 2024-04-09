using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Inventory;
using UnityEngine;

namespace Arcy.Quests
{
	[CreateAssetMenu(fileName = "new Quest", menuName = "Arcy/Quest/new Quest")]
	public class QuestSO : ScriptableObject
	{
		[field: SerializeField] public string questDisplayName;
		[field: SerializeField] public string guid { get; private set; }
		[field: SerializeField] private QuestManager questManager;

		[field: SerializeField] private QuestStateEnum _currentQuestState;
		[Header("Requirements")]
		[SerializeField] Quest[] requiredQuests;
		[SerializeField] Inventory.InventoryItem[] requiredItems;
		// [SerializeField] int requiredPlayerLvl;
		// [SerializeField] string[] requiredDialogues;
		// [SerializeField] Battle[] requiredBattles;
		[Header("Objectives")]
		[SerializeField] private GameObject[] _objectives;
		[Header("Reward")]
		[SerializeField] private QuestReward _questReward;

		[Serializable]
		public class QuestReward
		{
			[SerializeField] public int coin;
			[SerializeField] public int exp;
			[SerializeField] public GameObject[] items;
		}

		public void QuestStarted()
		{
			for (int i = 0; i < _objectives.Length; i++)
			{
				if (_objectives[i].TryGetComponent<QuestObjective>(out QuestObjective objective))
				{
					if (objective.isFinished)
					{
						continue;
					}
					else
					{
						objective.InitializeObjective(guid, i);

						if (objective.ThisObjectiveCanBeSkipped)
						{ continue; }
						else
						{ return; }
					}
				}
				else
				{
					Debug.LogError("No objective was found on GameObject" + _objectives[i].name);
					continue;
				}
			}
		}

		private void ObjectiveFinished(QuestObjective objective)
		{
			// Clear the current objective and Initialize the next one
			// if we've finished the final objective, finish the quest
		}

		private void Foo()
		{
			// Whenever "an event" happens, check whether we can switch the state of this quest.

			// Check all requirements to start quest

			// Check InventoryRequirements
			if (_currentQuestState == QuestStateEnum.REQUIREMENTS_NOT_MET)
			{
				InventorySO inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySO>(); // TODO - fix a proper inventory!

				if (requiredItems.Length > 0) // are there required items?
				{
					for (int i = 0; i < requiredItems.Length; i++)
					{
						foreach (InventorySlot itemSlot in inventory.itemSlots)
						{
							if (itemSlot.Item == requiredItems[i])
							{
								// 
							}
						}
					}
				}
			}
		}
	}
}
