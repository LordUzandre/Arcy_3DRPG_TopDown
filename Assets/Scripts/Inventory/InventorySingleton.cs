using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	public class InventorySingleton : ScriptableObject
	{
		[SerializeField] public Dictionary<InventoryItemBase, int> inventoryDictionary;

		private void AddItem(InventoryItemBase itemToAdd, int amountToAdd)
		{

		}

		private void RemoveItem(InventoryItemBase itemToRemove, int amountToRemove)
		{

		}
	}
}
