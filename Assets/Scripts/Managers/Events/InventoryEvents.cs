using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	public class InventoryEvents
	{
		public event Action<InventoryItem, int> onInventoryItemAdded;
		public void InventoryItemAdded(InventoryItem item, int amountAdded = 1)
		{
			if (onInventoryItemAdded != null)
			{
				onInventoryItemAdded(item, amountAdded);
			}

			InventoryUpdated();
		}

		public event Action<InventoryItem> onInventoryItemUsed;
		public void InventoryItemUsed(InventoryItem item)
		{
			if (onInventoryItemUsed != null)
			{
				onInventoryItemUsed(item);
			}

			InventoryUpdated();
		}

		// When an item is removed (like after a quest)
		public event Action<InventoryItem, int> onInventoryItemRemoved;
		public void InventoryItemRemoved(InventoryItem item, int amountUsed = 1)
		{
			if (onInventoryItemRemoved != null)
			{
				onInventoryItemRemoved(item, amountUsed);
			}

			InventoryUpdated();
		}

		public event Action onInventoryUpdated;
		public void InventoryUpdated()
		{
			if (onInventoryUpdated != null)
			{
				onInventoryUpdated.Invoke();
			}
		}
	}
}
