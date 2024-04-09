using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	[CreateAssetMenu(fileName = "Inventory SO", menuName = "Arcy/Inventory/new Inventory")]
	public class InventorySO : ScriptableObject
	{
		public int inventorySize;
		public InventorySlot[] itemSlots;

#if UNITY_EDITOR
		private void OnValidate()
		{
			inventorySize = itemSlots.Length;
		}
#endif
	}
}