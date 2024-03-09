using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	// [System.Serializable]
	// public class InventorySlot : MonoBehaviour
	// {
	// 	[SerializeField] public InventoryItem item;
	// 	[SerializeField] public int amount;
	// }

	[System.Serializable]
	public struct InventorySlot
	{
		[SerializeField] public InventoryItem item;
		[SerializeField] public int amount;
	}

}
