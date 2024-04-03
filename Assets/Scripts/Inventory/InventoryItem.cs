using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Inventory
{
	[CreateAssetMenu(fileName = "New Item", menuName = "Arcy/Inventory/Item", order = 60)]
	public class InventoryItem : ScriptableObject
	{
		[SerializeField] public string itemName;
		[SerializeField] public string guid;

		[Header("Components")]
		[SerializeField] Sprite inventoryIcon;
		[SerializeField] GameObject prefab;
		[Space]
		// [SerializeField] bool showInInventory = true;
		[SerializeField] bool stackable = false;
		[TextArea(3, 6)]
		[SerializeField] string description;

		// static so that there is only one cache, and not one for every item
		static Dictionary<string, InventoryItem> itemLookupCache;

		// PUBLIC:

		/// Get the inventory item instance from its UUID.
		/// String UUID that persists between game instances.
		/// Inventory item instance corresponding to the ID.
		public static InventoryItem GetFromID(string itemID)
		{
			// during first run:
			if (itemLookupCache == null)
			{
				itemLookupCache = new Dictionary<string, InventoryItem>();

				// Load all iventory items
				var itemList = Resources.LoadAll<InventoryItem>("");

				foreach (InventoryItem item in itemList)
				{
					if (itemLookupCache.ContainsKey(item.guid))
					{
						Debug.LogError("Looks like there's a duplicate GameDevTV.UI.InventorySystem ID for objects: " + itemLookupCache[item.guid] + " and " + item);
						continue;
					}

					itemLookupCache[item.guid] = item;
				}
			}

			// failsafe
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

		// Private:
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
	}
}
