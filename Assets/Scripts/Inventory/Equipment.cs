using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	public class Equipment : MonoBehaviour
	{
		// STATE
		Dictionary<EquipLocation, EquipableItem> equippedItems = new Dictionary<EquipLocation, EquipableItem>();

		// PUBLIC

		// Broadcasts when the items in the slots are added/removed.
		public event Action equipmentUpdated;

		// Return the item in the given equip location.
		public EquipableItem GetItemInSlot(EquipLocation equipLocation)
		{
			if (!equippedItems.ContainsKey(equipLocation))
			{
				return null;
			}

			return equippedItems[equipLocation];
		}

		// Add an item to the given equip location. Do not attempt to equip to
		// an incompatible slot.
		public void AddItem(EquipLocation slot, EquipableItem item)
		{
			Debug.Assert(item.GetAllowedEquipLocation() == slot);

			equippedItems[slot] = item;

			if (equipmentUpdated != null)
			{
				equipmentUpdated();
			}
		}

		// Remove the item for the given slot.
		public void RemoveItem(EquipLocation slot)
		{
			equippedItems.Remove(slot);
			if (equipmentUpdated != null)
			{
				equipmentUpdated();
			}
		}
	}
}
