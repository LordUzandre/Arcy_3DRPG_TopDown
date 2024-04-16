using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Inventory;
using UnityEngine;

namespace Arcy.Quests
{
	public class FetchObjective : QuestObjective
	{
		private bool _thisObjectiveCanBeSkipped;
		public override bool ThisObjectiveCanBeSkipped
		{
			get { return _thisObjectiveCanBeSkipped; }
			set { _thisObjectiveCanBeSkipped = value; }
		}

		[SerializeField] private InventoryItem _item;
		[SerializeField] private int requestedAmount = 1;

		public override void InitializeObjective(int questId, int objectiveIndex)
		{
			base.InitializeObjective(questId, objectiveIndex);

			InventoryUpdated();

			Management.GameManager.instance.gameEventManager.inventoryEvents.onInventoryUpdated += InventoryUpdated;
		}

		private void InventoryUpdated()
		{
			InventorySlot[] inventorySlots = gameObject.transform.Find("InventoryManager").GetComponent<InventoryManager>().slots;

			foreach (InventorySlot slot in inventorySlots)
			{
				if (slot.Item != _item)
				{
					if (slot.Amount >= requestedAmount)
					{
						// Finish Quest

						Debug.Log("PlaceholderQuest, FetchObjective: You now have the correct amount of required items to ");

						Management.GameManager.instance.gameEventManager.inventoryEvents.onInventoryUpdated -= InventoryUpdated;
						FinishObjective();
					}
				}
			}
		}

	}
}
