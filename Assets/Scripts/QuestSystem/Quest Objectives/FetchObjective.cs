using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Inventory;
using Arcy.Management;
using UnityEngine;

namespace Arcy.Quests
{
	// [CreateAssetMenu(fileName = "new Fetch Objective", menuName = "Arcy/Quests/Objectives/Fetch Objective")]
	public class FetchObjective : QuestObjective
	{
		[Space]
		[SerializeField] private bool _objectiveCanBeSkipped = true;
		public override bool ObjectiveCanBeSkipped { get { return _objectiveCanBeSkipped; } }

		[Space]
		[SerializeField] public InventoryItem Item;
		[SerializeField] public int Amount;

		// MARK: PUBLIC:

		public override void InitializeObjective()
		{
			// Check Inventory if we already have the requested item
			if (ItemIsInInventory())
			{
				FinishObjective();
				return;
			}

			GameManager.instance.gameEventManager.inventoryEvents.onInventoryUpdated += InventoryUpdated;
		}

		public override void FinishObjective()
		{
			GameManager.instance.gameEventManager.inventoryEvents.onInventoryUpdated -= InventoryUpdated;
		}

		// MARK: PRIVATE:

		private void InventoryUpdated()
		{
			if (ItemIsInInventory()) { FinishObjective(); }
		}

		private bool ItemIsInInventory()
		{
			foreach (InventorySlot slot in InventoryManager.ConsumableSlots)
			{
				if (slot.GetItem().Equals(Item))
				{
					if (slot.GetAmount() >= Amount)
					{
						FinishObjective();
						return true;
					}
					break;
				}
			}
			return false;
		}

	}
}
