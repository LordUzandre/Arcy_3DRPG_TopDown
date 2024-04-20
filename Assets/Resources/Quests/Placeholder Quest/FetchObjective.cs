using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Inventory;
using Arcy.Management;
using UnityEngine;

namespace Arcy.Quests
{
	[CreateAssetMenu(fileName = "new Fetch Objective", menuName = "Arcy/Quests/Objectives/Fetch Objective")]
	public class FetchObjective : QuestObjective
	{
		private bool _objectiveCanBeSkipped = true;
		[SerializeField] public override bool objectiveCanBeSkipped { get { return _objectiveCanBeSkipped; } set { _objectiveCanBeSkipped = value; } }

		[Space]
		[SerializeField] InventoryItem item;
		[SerializeField] int amount;

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
			foreach (InventorySlot slot in GameManager.instance.inventoryManager.slots)
			{
				if (slot.Item == item)
				{
					if (slot.Amount >= amount)
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
