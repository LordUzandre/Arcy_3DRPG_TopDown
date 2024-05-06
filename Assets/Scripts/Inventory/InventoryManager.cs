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
		[SerializeField] private StarterInventory _starterInventoryPrefab;
		[SerializeField] private bool _debugging;
		[SerializeField] private bool _useSaveData = true;

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
				if (slot.GetAmount() < 1)
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
				if (object.ReferenceEquals(consumableSlots[i].GetItem(), item))
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

			consumableSlots[i].SetItem(item);
			consumableSlots[i].AddtoAmount(amount);

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
				if (object.ReferenceEquals(consumableSlots[i].GetItem(), item))
				{
					return true;
				}
			}

			return false;
		}

		public InventoryItem GetItemInSlot(int slot)
		{
			return consumableSlots[slot].GetItem();
		}

		public int GetAmountInSlot(int slot)
		{
			return consumableSlots[slot].GetAmount();
		}

		public void RemoveFromSlot(int slot, int amount)
		{
			consumableSlots[slot].SubstractFromSlot(amount);
			if (consumableSlots[slot].GetAmount() <= 0)
			{
				consumableSlots[slot].SetAmount(0);
				consumableSlots[slot].SetItem(null);
			}

			GameManager.instance.gameEventManager.inventoryEvents.InventoryUpdated();
		}

		public bool AddItemToSlot(int slot, InventoryItem item, int amount)
		{
			if (consumableSlots[slot].GetItem() != item)
			{
				return AddToFirstEmptySlot(item, amount);
			}

			int i = FindStack(item);
			if (i >= 0)
			{
				slot = i;
			}

			consumableSlots[slot].SetItem(item);
			consumableSlots[slot].AddtoAmount(amount);

			GameManager.instance.gameEventManager.inventoryEvents.InventoryUpdated();

			return true;
		}

		// MARK: PRIVATE

		private InventorySlot[] LoadFromStarterInventory()
		{
			if (_starterInventoryPrefab != null)
			{
				// Get the original array
				InventorySlot[] originalArray = _starterInventoryPrefab.LoadStarterInventory();
				InventorySlot[] clonedArray = new InventorySlot[originalArray.Length];

				for (int i = 0; i < inventorySize; i++)
				{
					InventoryItem newItem = originalArray[i].GetItem();
					// Create a new InventorySlot instance with the cloned InventoryItem and amount
					clonedArray[i] = new InventorySlot(newItem, originalArray[i].GetAmount());
				}

				if (_debugging) Debug.Log("Loaded from Starter Inventory");

				return clonedArray;
			}
			else
			{
				Debug.LogError("Inventory Manager was unable to acquire items from starterInventory");
				return null;
			}
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
				if (consumableSlots[i].GetItem() == null)
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
				if (object.ReferenceEquals(consumableSlots[i].GetItem(), item))
				{
					return i;
				}
			}

			return -1;
		}

		// MARK: Save/Load
		public void SaveData(SaveData saveData)
		{
#if UNITY_EDITOR
			if (!_useSaveData)
			{
				return;
			}
#endif

			saveData.inventory.Clear();
			saveData.inventorySize = inventorySize;

			foreach (InventorySlot slot in consumableSlots)
			{
				if (slot.GetItem() != null && slot.GetAmount() > 0)
				{
					if (saveData.inventory.ContainsKey(slot.GetItem().guid))
					{
						saveData.inventory.Remove(slot.GetItem().guid);
					}

					saveData.inventory.Add(slot.GetItem().guid, slot.GetAmount());
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

#if UNITY_EDITOR
			if (!_useSaveData)
			{
				return;
			}
#endif

			// Load from LoadData
			List<InventorySlot> consumablesToBeAdded = new List<InventorySlot>();

			foreach (KeyValuePair<int, int> itemID in loadData.inventory)
			{
				if (itemID.Key == 0 || itemID.Value <= 0)
				{
					continue;
				}

				InventorySlot newSlot = new InventorySlot(InventoryItem.GetFromID(itemID.Key), itemID.Value);

				if (newSlot.GetItem() == null)
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
