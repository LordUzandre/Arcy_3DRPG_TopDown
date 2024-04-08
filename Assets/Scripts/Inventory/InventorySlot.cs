using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	[System.Serializable]
	public struct InventorySlot
	{
		[SerializeField] public InventoryItem item;
		[SerializeField] public int amount;
		[SerializeField] public GameObject prefab;
	}
}
