using System.Collections;
using System.Collections.Generic;
using Arcy.Management;
using Arcy.Saving;
using UnityEngine;

namespace Arcy.Inventory
{
	public class InventoryManager : MonoBehaviour, ISaveableEntity
	{
		[Header("Config")]
		[SerializeField] InventorySO starterInventory;

		[Space]
		[SerializeField] public int inventorySize = 16;
		[SerializeField] public InventorySlot[] equipmentSlots;
		[SerializeField] public InventorySlot[] consumableSlots;

		// MARK: PUBLIC

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
			foreach (InventorySlot slot in consumableSlots)
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
			return consumableSlots.Length;
		}

		/// <summary>
		/// Is there an instance of the item in the inventory?
		/// </summary>
		public bool HasItem(InventoryItem item)
		{
			for (int i = 0; i < consumableSlots.Length; i++)
			{
				if (object.ReferenceEquals(consumableSlots[i].Item, item))
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
				return false;
			}

			consumableSlots[i].Item = item;
			consumableSlots[i].Amount += amount;

			GameManager.instance.gameEventManager.inventoryEvents.InventoryUpdated();

			return true;
		}

		/// <summary>
		/// Is there an instance of the item in the inventory?
		/// </summary>
		public bool InventoryHoldsItem(InventoryItem item)
		{
			for (int i = 0; i < consumableSlots.Length; i++)
			{
				if (object.ReferenceEquals(consumableSlots[i].Item, item))
				{
					return true;
				}
			}

			return false;
		}

		public InventoryItem GetItemInSlot(int slot)
		{
			return consumableSlots[slot].Item;
		}

		public int GetAmountInSlot(int slot)
		{
			return consumableSlots[slot].Amount;
		}

		public void RemoveFromSlot(int slot, int amount)
		{
			consumableSlots[slot].Amount -= amount;
			if (consumableSlots[slot].Amount <= 0)
			{
				consumableSlots[slot].Amount = 0;
				consumableSlots[slot].Item = null;
			}

			GameManager.instance.gameEventManager.inventoryEvents.InventoryUpdated();
		}

		public bool AddItemToSlot(int slot, InventoryItem item, int amount)
		{
			if (consumableSlots[slot].Item != item)
			{
				return AddToFirstEmptySlot(item, amount);
			}

			int i = FindStack(item);
			if (i >= 0)
			{
				slot = i;
			}

			consumableSlots[slot].Item = item;
			consumableSlots[slot].Amount += amount;

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
			for (int i = 0; i < consumableSlots.Length; i++)
			{
				if (consumableSlots[i].Item == null)
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
				Debug.Log("First Empty Slot Found: " + i);
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

			for (int i = 0; i < consumableSlots.Length; i++)
			{
				if (object.ReferenceEquals(consumableSlots[i].Item, item))
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

			foreach (InventorySlot slot in consumableSlots)
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
				consumableSlots = LoadFromStarterInventory();
				return;
			}

			// Load from LoadData
			List<InventorySlot> consumablesToBeAdded = new List<InventorySlot>();

			foreach (KeyValuePair<int, int> itemID in loadData.inventory)
			{
				if (itemID.Key == 0 || itemID.Value <= 0)
				{
					continue;
				}

				InventorySlot newSlot = new InventorySlot(InventoryItem.GetFromID(itemID.Key), itemID.Value);

				if (newSlot.Item == null)
				{
					Debug.LogError("Unable to access item: " + itemID.Key);
					continue;
				}

				consumablesToBeAdded.Add(newSlot);
			}

			if (consumablesToBeAdded.Count < inventorySize)
			{
				int remainingSlots = inventorySize - consumablesToBeAdded.Count;
				for (int i = 0; i < remainingSlots; i++)
				{
					consumablesToBeAdded.Add(null);
				}
			}

			consumableSlots = consumablesToBeAdded.ToArray();
		}
	}
}
