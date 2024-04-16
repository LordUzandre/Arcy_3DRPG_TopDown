using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Inventory
{
	[CreateAssetMenu(fileName = "New Item", menuName = "Arcy/Inventory/Item", order = 60)]
	public class InventoryItem : ScriptableObject
	{
		[SerializeField] public string itemName = "";
		[SerializeField] public int guid = 0;

		[Header("Components")]
		[SerializeField] public GameObject prefab;
		[SerializeField] Sprite inventoryIcon;
		[Space]
		// [SerializeField] bool showInInventory = true;
		[SerializeField] bool stackable = false;
		[TextArea(3, 6)]
		[SerializeField] string description;

		// static so that there is only one cache, and not one for every item
		static Dictionary<int, InventoryItem> itemLookupCache;

		// PUBLIC:

		/// Get the inventory item instance from its GUID.
		/// Int GUID that persists between game instances.
		/// Inventory item instance corresponding to the ID.
		public static InventoryItem GetFromID(int itemID)
		{
			// during first run:
			if (itemLookupCache == null)
			{
				itemLookupCache = new Dictionary<int, InventoryItem>();

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
			if (itemID == 0 || !itemLookupCache.ContainsKey(itemID)) return null;

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

		public int GetGuid()
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
		private void OnValidate()
		{
			if (itemName != name)
			{
				itemName = name;
			}

			if (guid == 0)
			{
				guid = Mathf.Abs(System.Guid.NewGuid().GetHashCode());
			}
		}
#endif
	}
}
