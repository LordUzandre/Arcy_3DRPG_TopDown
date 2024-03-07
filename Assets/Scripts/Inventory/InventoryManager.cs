using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	// [CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Inventory")]
	public class InventoryManager : ScriptableObject
	{
		[SerializeField] public List<InventoryItem> items;
		[SerializeField] public int inventorySize = 16;
		[SerializeField] public int currentlyUsedSlots;

		public Action<InventoryItem> newItemAdded;

		private void AddItem(InventoryItem itemToAdd, int amountToAdd)
		{
			if (currentlyUsedSlots >= inventorySize)
			{
				// Check if there is space for it.
			}

			// Check if there is already an instance of the item in inventory,
			// if not, add a new slot for it.
		}

		private void UseItem(InventoryItem itemToUse)
		{

		}

		private void RemoveItem(InventoryItem itemToRemove, int amountToRemove)
		{
			// If inventoryslot is 0, remove the slot from inventory
		}
	}
}
