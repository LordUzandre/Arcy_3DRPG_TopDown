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
		[SerializeField] string itemName;
		[SerializeField] string guid;

		[Header("Components")]
		[SerializeField] Sprite inventoryIcon;
		[SerializeField] GameObject prefab;
		[Space]
		[SerializeField] bool showInInventory = true;
		[SerializeField] bool stackable = false;
		[TextArea(1, 3)]
		[SerializeField] string description;

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

		// STATE
		static Dictionary<string, InventoryItem> itemLookupCache;

		// PUBLIC

		/// <summary>
		/// Get the inventory item instance from its UUID.
		/// </summary>
		/// <param name="itemID">
		/// String UUID that persists between game instances.
		/// </param>
		/// <returns>
		/// Inventory item instance corresponding to the ID.
		/// </returns>
		public static InventoryItem GetFromID(string itemID)
		{
			if (itemLookupCache == null)
			{
				itemLookupCache = new Dictionary<string, InventoryItem>();
				var itemList = Resources.LoadAll<InventoryItem>("");
				foreach (InventoryItem item in itemList)
				{
					if (itemLookupCache.ContainsKey(item.guid))
					{
						Debug.LogError(string.Format("Looks like there's a duplicate GameDevTV.UI.InventorySystem ID for objects: {0} and {1}", itemLookupCache[item.guid], item));
						continue;
					}

					itemLookupCache[item.guid] = item;
				}
			}

			if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
			return itemLookupCache[itemID];
		}

		// Called when the item is pressed in the inventory
		public virtual void Use()
		{
			// Use the item
			// Something may happen
		}

		public Sprite GetIcon()
		{
			return inventoryIcon;
		}

		public string GetGuid()
		{
			return guid;
		}

		public bool IsStackable()
		{
			return stackable;
		}

		public string GetDisplayName()
		{
			return itemName;
		}

		public string GetDescription()
		{
			return description;
		}
	}
}
