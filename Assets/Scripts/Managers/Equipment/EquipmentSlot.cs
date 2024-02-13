using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Arcy.Inventory
{
	public class EquipmentSlot : MonoBehaviour
	{
		public Image icon;
		public Button removeButton;

		InventoryItemBase item; // Current item in the slot

		// Add item to the slot
		public void AddItem(InventoryItemBase newItem)
		{
			item = newItem;

			icon.sprite = item.inventoryIcon;
			icon.enabled = true;
			removeButton.interactable = true;
		}

		// Clear the slot
		public void ClearSlot()
		{
			item = null;

			icon.sprite = null;
			icon.enabled = false;
			removeButton.interactable = false;
		}

		// If the remove button is pressed, this function will be called.
		public void RemoveItemFromInventory()
		{
			InventorySingleton.instance.Remove(item);
		}

		// Use the item
		public void UseItem()
		{
			if (item != null)
			{
				item.Use();
			}
		}
	}
}
