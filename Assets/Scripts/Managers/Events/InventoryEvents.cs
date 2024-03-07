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
	}
}
