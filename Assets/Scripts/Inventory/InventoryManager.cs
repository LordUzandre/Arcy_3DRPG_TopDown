using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Management;
using Arcy.Saving;
using Arcy.Utils;
using UnityEngine;

namespace Arcy.Inventory
{
	public class InventoryManager : MonoBehaviour, ISaveableEntity
	{
		[Header("Config")]
		[SerializeField] bool loadFromTempInventory;
		[SerializeField] InventorySO tempInventory;

		[Space]
		// Use for save-data

		[SerializeField] public int inventorySize = 16;
		[SerializeField] public InventorySlot[] slots;
		[SerializeField] public SerializableDictionary<InventoryItem, int> mySerializableDictionary = new SerializableDictionary<InventoryItem, int>();

		// MARK: Public:
		public static InventoryManager GetPlayerInventory()
		{
			return null;
		}

		public bool HasSpaceFor(InventoryItem item)
		{
			return false;
		}

		/// <summary>
		/// How many slots are in the inventory?
		/// </summary>
		public int GetSize()
		{
			return slots.Length;
		}

		/// <summary>
		/// Attempt to add the items to the first available slot.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <param name="amount">The number to add.</param>
		/// <returns>Whether or not the item could be added.</returns>
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

		/// <summary>
		/// Return the item type in the given slot.
		/// </summary>
		public InventoryItem GetItemInSlot(int slot)
		{
			return slots[slot].Item;
		}

		/// <summary>
		/// Get the number of items in the given slot.
		/// </summary>
		public int GetAmountInSlot(int slot)
		{
			return slots[slot].Amount;
		}

		/// <summary>
		/// Remove a number of items from the given slot. Will never remove more
		/// that there are.
		/// </summary>
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

		private void Awake()
		{
#if UNITY_EDITOR
			if (loadFromTempInventory)
			{
				slots = new InventorySlot[tempInventory.inventorySize];

				for (int i = 0; i < slots.Length; i++)
				{
					if (tempInventory.itemSlots[i].Item == null || tempInventory.itemSlots[i].Amount < 1)
					{
						// We've reached the end of the items
						break;
					}
					else
					{
						AddItemToSlot(i, tempInventory.itemSlots[i].Item, tempInventory.itemSlots[i].Amount);
					}
				}
			}
#else
			// Load from SaveData
			slots = new InventorySlot[inventorySize];
#endif

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
			foreach (InventorySlot slot in slots)
			{
				if (slot.Item != null)
				{
					if (slot.Item == item)
					{
						AddItemToSlot(FindSlot(item), item, amount);
					}
				}
				else
				{
					continue;
				}

			}

			AddToFirstEmptySlot(item, amount);
		}

		/// <summary>
		/// Find an empty slot.
		/// </summary>
		/// <returns>-1 if all slots are full.</returns>
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
		/// Find a slot that can accomodate the given item.
		/// </summary>
		/// <returns>-1 if no slot is found.</returns>
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
		/// Find an existing stack of this item type.
		/// </summary>
		/// <returns>-1 if no stack exists or if the item is not stackable.</returns>
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

		// Struct to be used by save-system
		[Serializable]
		private struct InventorySlotRecord
		{
			public string itemID;
			public int amount;
		}

		// MARK: Save/Load
		// object ISaveable.CaptureState()
		public void SaveData(SaveData data)
		{
			data.inventory.Clear();

			foreach (InventorySlot slot in slots)
			{
				if (slot.Item != null && slot.Amount < 0)
				{
					if (data.inventory.ContainsKey(slot.Item.guid))
					{
						data.inventory.Remove(slot.Item.guid);
					}

					data.inventory.Add(slot.Item.guid, slot.Amount);
				}
			}
		}

		// void ISaveable.RestoreState(object state)
		public void LoadData(SaveData data)
		{
			// var slotStrings = (InventorySlotRecord[])state;
			for (int i = 0; i < inventorySize; i++)
			{
				// slots[i].item = InventoryItem.GetFromID(slotStrings[i].itemID);
				// slots[i].amount = slotStrings[i].amount;
			}

			GameManager.instance.gameEventManager.inventoryEvents.InventoryUpdated();
		}
	}
}
