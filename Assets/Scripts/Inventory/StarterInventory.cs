using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	public class StarterInventory : MonoBehaviour
	{
		[SerializeField] private InventorySlot[] _starterConsumableSlots = new InventorySlot[16];
		[SerializeField] private InventorySlot[] _starterEquipmentSlots = new InventorySlot[8];

		public InventorySlot[] LoadStarterInventory()
		{
			return _starterConsumableSlots;
		}

		public InventorySlot[] LoadEquipmentSlots()
		{
			return _starterEquipmentSlots;
		}
	}
}
