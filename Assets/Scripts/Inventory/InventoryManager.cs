using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Management;
using Arcy.Saving;
using Arcy.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arcy.Inventory
{
	public class InventoryManager : MonoBehaviour, ISaveableEntity
	{
		[Header("Config")]
		[SerializeField] InventorySO starterInventory;

		[Space]
		[SerializeField] public int inventorySize = 16;
		[SerializeField] public static InventorySlot[] slots;

		// MARK: PUBLIC:

		// Single item
		public bool HasSpaceFor(InventoryItem item)
		{
			return FindSlot(item) >= 0;
		}

		// Multiple items
		public bool HasSpaceFor(IEnumerable<InventoryItem> items)
		{
			int freeSlots = AvailableSlots();

			List<InventoryItem> stackedItems = new List<InventoryItem>();

			foreach (InventoryItem item in items)
			{
				if (item.IsStackable())
				{
					if (HasItem(item)) continue;
					if (stackedItems.Contains(item)) continue;
					stackedItems.Add(item);
				}

				// Already seen in the list
				if (freeSlots <= 0) return false;

				freeSlots--;
			}
			return true;
		}

		/// <summary>
		/// How many free slots are available in the inventory.
		/// </summary>
		public int AvailableSlots()
		{
			int count = 0;
			foreach (InventorySlot slot in slots)
			{
				if (slot.Amount < 1)
				{
					count++;
				}
			}
			return count;
		}

		public int GetInventorySize()
		{
			return slots.Length;
		}

		/// <summary>
		/// Is there an instance of the item in the inventory?
		/// </summary>
		public bool HasItem(InventoryItem item)
		{
			for (int i = 0; i < slots.Length; i++)
			{
				if (object.ReferenceEquals(slots[i].Item, item))
				{
					return true;
				}
			}
			return false;
		}

		public bool AddToFirstEmptySlot(InventoryItem item, int amount)
		{
			int i = FindSlot(item);

			if (i < 0)
			{
				Debug.Log("Unable to find free ");
				return false;
			}

			slots[i].Item = item;
			slots[i].Amount += amount;

			GameManager.instance.gameEventManager.inventoryEvents.InventoryUpdated();

			return true;
		}

		/// <summary>
		/// Is there an instance of the item in the inventory?
		/// </summary>
		public bool InventoryHoldsItem(InventoryItem item)
		{
			for (int i = 0; i < slots.Length; i++)
			{
				if (object.ReferenceEquals(slots[i].Item, item))
				{
					return true;
				}
			}

			return false;
		}

		public InventoryItem GetItemInSlot(int slot)
		{
			return slots[slot].Item;
		}

		public int GetAmountInSlot(int slot)
		{
			return slots[slot].Amount;
		}

		public void RemoveFromSlot(int slot, int amount)
		{
			slots[slot].Amount -= amount;
			if (slots[slot].Amount <= 0)
			{
				slots[slot].Amount = 0;
				slots[slot].Item = null;
			}

			GameManager.instance.gameEventManager.inventoryEvents.InventoryUpdated();
		}

		/// <summary>
		/// Will add an item to the given slot if possible. If there is already
		/// a stack of this type, it will add to the existing stack. Otherwise,
		/// it will be added to the first empty slot.
		/// </summary>
		/// <param name="slot">The slot to attempt to add to.</param>
		/// <param name="item">The item type to add.</param>
		/// <param name="amount">The number of items to add.</param>
		/// <returns>True if the item was added anywhere in the inventory.</returns>
		public bool AddItemToSlot(int slot, InventoryItem item, int amount)
		{
			if (slots[slot].Item != null)
			{
				return AddToFirstEmptySlot(item, amount); ;
			}

			int i = FindStack(item);
			if (i >= 0)
			{
				slot = i;
			}

			slots[slot].Item = item;
			slots[slot].Amount += amount;

			GameManager.instance.gameEventManager.inventoryEvents.InventoryUpdated();

			return true;
		}


		// MARK: PRIVATE

		private InventorySlot[] LoadFromStarterInventory()
		{
			return starterInventory.itemSlots;
		}

		private void OnEnable()
		{
			GameManager.instance.gameEventManager.inventoryEvents.onInventoryItemAdded += AddPickup;
		}

		private void OnDisable()
		{
			GameManager.instance.gameEventManager.inventoryEvents.onInventoryItemAdded += AddPickup;
		}

		private void AddPickup(InventoryItem item, int amount)
		{
			AddItemToSlot(FindSlot(item), item, amount);
		}

		/// <summary>
		/// Find an empty slot. returns -1 if all slots are full.
		/// </summary>
		private int FindEmptySlot()
		{
			for (int i = 0; i < slots.Length; i++)
			{
				if (slots[i].Item == null)
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Find a slot that can accomodate the given item. Returns -1 if no slot is found.
		/// </summary>
		private int FindSlot(InventoryItem item)
		{
			int i = FindStack(item);
			if (i < 0)
			{
				i = FindEmptySlot();
			}
			return i;
		}

		/// <summary>
		/// Find an existing stack of this item type. Returns -1 if no stack exists or if the item is not stackable.
		/// </summary>
		private int FindStack(InventoryItem item)
		{
			if (!item.IsStackable())
			{
				return -1;
			}

			for (int i = 0; i < slots.Length; i++)
			{
				if (object.ReferenceEquals(slots[i].Item, item))
				{
					return i;
				}
			}

			return -1;
		}

		// MARK: Save/Load
		public void SaveData(SaveData saveData)
		{
			saveData.inventorySize = inventorySize;
			// saveData.inventory.Clear();

			foreach (InventorySlot slot in slots)
			{
				if (slot.Item != null && slot.Amount > 0)
				{
					if (saveData.inventory.ContainsKey(slot.Item.guid))
					{
						saveData.inventory.Remove(slot.Item.guid);
					}

					saveData.inventory.Add(slot.Item.guid, slot.Amount);
				}
			}
		}

		public void LoadData(SaveData loadData)
		{
			if (loadData.inventory.Count < 1)
			{
				slots = LoadFromStarterInventory();
				return;
			}

			List<InventorySlot> inventoryToBeAdded = new List<InventorySlot>();
			int count = 0;

			foreach (int itemID in loadData.inventory.Keys)
			{
				if (itemID == 0 || loadData.inventorySize < count)
				{
					break;
				}

				InventorySlot newSlot = new InventorySlot();

				newSlot.Item = InventoryItem.GetFromID(itemID);
				// Debug.Log(newSlot.Item.GetDisplayName() + " added to inventory");

				if (newSlot.Item != null)
				{
					newSlot.Amount = loadData.inventory[itemID];
				}

				if (newSlot.Item != null && newSlot.Amount > 0)
				{
					inventoryToBeAdded.Add(newSlot);
				}

				count++;

			}

			slots = inventoryToBeAdded.ToArray();
			// GameManager.instance.gameEventManager.inventoryEvents.InventoryUpdated();
		}
	}
}
