using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
	public class InventoryItemBase : ScriptableObject
	{
		public string itemName;
		public Sprite inventoryIcon;
		public bool showInInventory = true;

		// Called when the item is pressed in the inventory
		public virtual void Use()
		{
			// Use the item
			// Something may happen
		}

		// Call this method to remove the item from inventory
		public void RemoveFromInventory()
		{
			InventorySingleton.instance.Remove(this);
		}
	}
}
