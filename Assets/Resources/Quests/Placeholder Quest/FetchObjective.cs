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
		// [SerializeField] private int requestedAmount = 1;

		public override void Initialize(string questId, int objectiveIndex, string questObjectiveState)
		{
			base.Initialize(questId, objectiveIndex, questObjectiveState);

			InventoryUpdated(_item);

			Management.GameEventManager.instance.inventoryEvents.onInventoryUpdated += InventoryUpdated;
		}

		private void InventoryUpdated(Inventory.InventoryItem item)
		{
			if (item != _item)
			{
				return;
			}

			// if numberInInventory >= requestedAmount

			Management.GameEventManager.instance.inventoryEvents.onInventoryUpdated -= InventoryUpdated;
			FinishObjective();
		}


	}
}
