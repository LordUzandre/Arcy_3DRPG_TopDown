using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	public class InventoryEvents
	{
		public event Action<InventoryItem> onInventoryItemAdded;
		public void InventoryItemAdded(InventoryItem item)
		{
			if (onInventoryItemAdded != null)
			{
				onInventoryItemAdded(item);
			}
		}

		public event Action<InventoryItem> onInventoryItemUsed;
		public void InventoryItemUsed(InventoryItem item)
		{
			if (onInventoryItemUsed != null)
			{
				onInventoryItemUsed(item);
			}
		}

		// When an item is removed (like after a quest)
		public event Action<InventoryItem, int> onInventoryItemRemoved;
		public void InventoryItemRemoved(InventoryItem item, int amount = 1)
		{
			int myAmount = amount;

			if (onInventoryItemRemoved != null)
			{
				onInventoryItemRemoved(item, myAmount);
			}
		}
	}
}
