using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	// [CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Inventory")]
	public class InventoryManager : MonoBehaviour
	{
		public List<InventorySlot> inventory;
		[SerializeField] public int inventorySize = 16;
		[SerializeField] public int currentlyUsedSlots;

		[SerializeField] private EditorInventory _editorInventory;

		public Action<InventoryItem> newItemAdded;

		private void Start()
		{
#if UNITY_EDITOR
			CreateInventoryFromTemp();
#else
			CreateInventoryFromSaveData();
			// TODO - Load from SaveData
#endif
		}

		private List<InventorySlot> CreateInventoryFromTemp()
		{

			// Load from a temp Editor-friendly Inventory
			if (_editorInventory != null)
			{
				InventorySlot[] items = _editorInventory.inventorySlots;
				List<InventorySlot> tempInventory = new List<InventorySlot>(items.Length);

				if (items.Length > -1)
				{
					foreach (InventorySlot item in items)
					{
						if (item.item != null)
						{
							tempInventory.Add(item);
							// Debug.Log("Inventory Item Added: " + item.item.itemName);
							// Debug.Log("Length = " + items.Length);
						}
					}
				}

				return tempInventory;

			}
			else
			{
				return null;
			}
		}

		private void CreateInventoryFromSaveData()
		{

		}

		private void AddItem(InventoryItem itemToAdd, int amountToAdd)
		{
			if (currentlyUsedSlots >= inventorySize)
			{
				// Check if there is space for it.
			}

			// foreach (InventoryItem item in items)
			// {
			// 	if (item == itemToAdd)
			// 	{

			// 		return;
			// 	}
			// }

			// Check if there is already an instance of the item in inventory,
			// if not, add a new slot for it.
		}

		private void RemoveItem(InventoryItem itemToRemove, int amountToRemove)
		{
			// If inventoryslot is 0, remove the slot from inventory
		}
	}
}
