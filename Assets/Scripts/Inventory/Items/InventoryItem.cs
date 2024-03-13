using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Inventory
{
	[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 60)]
	public class InventoryItem : ScriptableObject
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
	}
}
