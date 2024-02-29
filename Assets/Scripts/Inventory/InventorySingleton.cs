using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	// [CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Inventory")]
	public class InventorySingleton : ScriptableObject
	{
		[SerializeField] public List<InventoryItem> items;
		//[SerializeField] private List<InventoryItemBase> _database;
		[SerializeField] public int inventorySize = 16;
		[SerializeField] public int currentlyUsedSlots;

		private void AddItem(InventoryItemBase itemToAdd, int amountToAdd)
		{
			if (currentlyUsedSlots >= inventorySize)
			{
				// Check if there is space for it.
			}

			// Check if there is already an instance of the item in inventory,
			// if not, add a new slot for it.
		}

		private void UseItem(InventoryItemBase itemToUse)
		{

		}

		private void RemoveItem(InventoryItemBase itemToRemove, int amountToRemove)
		{
			// If inventoryslot is 0, remove the slot from inventory
		}
	}

	[Serializable]
	public class InventoryItem
	{
		[SerializeField] public InventoryItemBase item;
		[SerializeField] public int amount;
	}
}
