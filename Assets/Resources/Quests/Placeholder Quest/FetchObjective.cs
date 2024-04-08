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

		public override void InitializeObjective(string questId, int objectiveIndex)
		{
			base.InitializeObjective(questId, objectiveIndex);

			InventoryUpdated();

			Management.GameEventManager.instance.inventoryEvents.onInventoryUpdated += InventoryUpdated;
		}

		private void InventoryUpdated()
		{
			InventorySlot[] inventorySlots = gameObject.transform.Find("InventoryManager").GetComponent<InventoryManager>().slots;

			foreach (InventorySlot slot in inventorySlots)
			{
				if (slot.item != _item)
				{
					if (slot.amount >= requestedAmount)
					{
						// Finish Quest

						Debug.Log("PlaceholderQuest, FetchObjective: You now have the correct amount of required items to ");

						Management.GameEventManager.instance.inventoryEvents.onInventoryUpdated -= InventoryUpdated;
						FinishObjective();
					}
				}
			}
		}

	}
}
