using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	public class EditorInventory : ScriptableObject
	{
		[SerializeField] public InventorySlot[] inventorySlots;
	}
}
