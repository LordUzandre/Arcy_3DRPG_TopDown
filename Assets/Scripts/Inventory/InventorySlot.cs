using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	[System.Serializable]
	public struct InventorySlot
	{
		[SerializeField] private InventoryItem _item;
		public InventoryItem Item { get { return _item; } set { _item = value; } }

		[SerializeField] private int _amount;
		public int Amount { get { return _amount; } set { _amount = value; } }
	}
}
