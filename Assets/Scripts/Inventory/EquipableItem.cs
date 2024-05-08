using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	/// <summary>
	/// An inventory item that can be equipped to the player. Weapons could be a
	/// subclass of this.
	/// </summary>
	[CreateAssetMenu(menuName = ("Arcy/Inventory/Equipable Item"))]
	public class EquipableItem : InventoryItem
	{
		[Space]
		[SerializeField] EquipLocation allowedEquipLocation = EquipLocation.RightHand;

		// Public:
		public EquipLocation GetAllowedEquipLocation()
		{
			return allowedEquipLocation;
		}
	}
}
