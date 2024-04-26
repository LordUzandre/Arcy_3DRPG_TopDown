using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	[System.Serializable]
	public class InventorySlot
	{
		[SerializeField] private InventoryItem _item;
		public InventoryItem Item { get { return _item; } set { _item = value; } }

		[SerializeField] private int _amount;
		public int Amount { get { return _amount; } set { _amount = value; } }

		public InventorySlot(InventoryItem item, int amount)
		{
			_item = item;
			_amount = amount;
		}
	}
}
