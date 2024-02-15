using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Inventory
{
	public class InventorySingleton : MonoBehaviour
	{
		#region Singleton

		public static InventorySingleton instance;

		void Awake()
		{
			instance = this;
		}

		#endregion

		public System.Action onItemChangedCallback;

		public int space = 10;  // Amount of item spaces

		// Our current list of items in the inventory
		public List<InventoryItemBase> items = new List<InventoryItemBase>();

		// Add a new item if enough room
		public void Add(InventoryItemBase item)
		{
			if (item.showInInventory)
			{
				if (items.Count >= space)
				{
					Debug.Log("Not enough room.");
					return;
				}

				items.Add(item);

				if (onItemChangedCallback != null)
					onItemChangedCallback.Invoke();
			}
		}

		// Remove an item
		public void Remove(InventoryItemBase item)
		{
			items.Remove(item);

			if (onItemChangedCallback != null)
				onItemChangedCallback.Invoke();
		}
	}
}
