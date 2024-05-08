using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	public class StarterInventory : MonoBehaviour
	{
		[SerializeField] private InventorySlot[] _starterConsumableSlots = new InventorySlot[16];
		[SerializeField] private EquipmentSlot[] _starterEquipmentSlots = new EquipmentSlot[4];

		public InventorySlot[] LoadStarterInventory()
		{
			return _starterConsumableSlots;
		}

		public EquipmentSlot[] LoadEquipmentSlots()
		{
			return _starterEquipmentSlots;
		}

#if UNITY_EDITOR
		public EquipmentSlot[] SetInitialEquipment()
		{
			EquipmentSlot[] equipmentArray = new EquipmentSlot[3];
			equipmentArray[0].EquipmentLocation = EquipLocation.RightHand;
			equipmentArray[1].EquipmentLocation = EquipLocation.LeftHand;
			equipmentArray[2].EquipmentLocation = EquipLocation.Outfit;
			equipmentArray[3].EquipmentLocation = EquipLocation.Helmet;
			return equipmentArray;
		}
#endif
	}
}
