using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	public class InventoryEvents
	{
		public event Action<InventoryItemBase> onInventoryItemAdded;
		public void InventoryItemAdded(InventoryItemBase item)
		{
			if (onInventoryItemAdded != null)
			{
				onInventoryItemAdded(item);
			}
		}

		public event Action<InventoryItemBase> onInventoryItemUsed;
		public void InventoryItemUsed(InventoryItemBase item)
		{
			if (onInventoryItemUsed != null)
			{
				onInventoryItemUsed(item);
			}
		}
	}
}
