using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	[CreateAssetMenu(fileName = "Inventory SO", menuName = "Arcy/Inventory/new Inventory")]
	public class InventorySO : ScriptableObject
	{
		public InventorySlot[] itemSlots;
	}
}