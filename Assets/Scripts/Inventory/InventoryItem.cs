using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Arcy.Inventory
{
	[CreateAssetMenu(fileName = "New Item", menuName = "Arcy/Inventory/Item", order = 60)]
	public class InventoryItem : ScriptableObject
	{
		private string _itemName = "";
		private int _guid;
		[SerializeField] public bool showInInventory = true; // Used by UI

		[Header("UI")]
		private Sprite _inventoryIcon;
		[Space]
		[SerializeField] public bool stackable = false;
		[TextArea(3, 6)]
		[SerializeField] private string _description;

		// static so that there is only one cache, and not one for every item
		private static Dictionary<int, InventoryItem> _itemLookupCache;

		/*
		// MARK: PUBLIC:
		------------------------------------------------------------------------------
		*/

		// Called when the item is pressed in the inventory
		public virtual void Use()
		{
			// Use the item
			// Something may happen
		}

		/*
		// MARK: GETTERS:
		------------------------------------------------------------------------------
		*/

		/// Get the inventory item instance from its GUID.
		public static InventoryItem GetFromID(int itemID)
		{
			// during first run:
			if (_itemLookupCache == null)
			{
				_itemLookupCache = new Dictionary<int, InventoryItem>();

				IEnumerable itemList = Resources.LoadAll<InventoryItem>("");

				foreach (InventoryItem item in itemList)
				{
					if (_itemLookupCache.ContainsKey(item._guid))
					{
						Debug.LogError("There's a duplicate InventoryItemID for objects: " + _itemLookupCache[item._guid] + " and " + item);
						continue;
					}

					_itemLookupCache[item._guid] = item;
				}
			}

			// failsafe
			if (itemID == 0 || !_itemLookupCache.ContainsKey(itemID)) return null;

			return _itemLookupCache[itemID];
		}

		public bool IsStackable() { return stackable; }

		public Sprite GetIcon() { return _inventoryIcon; }

		public int GetGuid() { return _guid; }

		public string GetItemName() { return _itemName; }

		public string GetDescription() { return _description; }

		/*
		// PRIVATE:
		------------------------------------------------------------------------------
		*/

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_guid == 0) _guid = Utils.GuidGenerator.guid(this);
			if (_itemName != name) _itemName = name;
		}
#endif

	}

	/*
	// MARK: EDITOR:
	------------------------------------------------------------------------------
	*/

#if UNITY_EDITOR
	[CustomEditor(typeof(InventoryItem))]
	public class ItemEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			InventoryItem item = (InventoryItem)target;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Item Name", item.GetItemName().ToString(), EditorStyles.whiteLabel, GUILayout.ExpandHeight(true));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Guid", item.GetGuid().ToString(), EditorStyles.whiteLabel, GUILayout.ExpandHeight(true));
			EditorGUILayout.EndHorizontal();

			base.DrawDefaultInspector();
		}
	}
#endif

}
