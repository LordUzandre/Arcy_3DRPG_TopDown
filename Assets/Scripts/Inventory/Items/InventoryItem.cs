using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Inventory
{
	[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 60)]
	public class InventoryItem : ScriptableObject
	{
		public string itemName;
		public string guid;

		[Header("Components")]
		public Sprite inventoryIcon;
		public GameObject prefab;
		[Space]
		public bool showInInventory = true;

#if UNITY_EDITOR
		// Generate a unique identifier
		[ContextMenu("Generate Unique Identifier (guid)")]
		private void GenerateGuid() { guid = System.Guid.NewGuid().ToString(); }

		private void OnValidate()
		{
			if (guid == null)
				GenerateGuid();
		}
#endif

		// Called when the item is pressed in the inventory
		public virtual void Use()
		{
			// Use the item
			// Something may happen
		}
	}
}
