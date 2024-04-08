using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Management;
using UnityEngine;

namespace Arcy.Inventory
{
	public class InventoryManager : MonoBehaviour
	{
		[Header("Config")]
		[SerializeField] bool loadInventoryFromSaveData;

		[Space]
		// Use for save-data
		public InventorySO inventory;

		// Set the size of the inventory
		[SerializeField] public int inventorySize = 16;
		// STATE
		[SerializeField] public InventorySlot[] slots;

		// Public:
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

			slots[i].item = item;
			slots[i].amount += amount;

			GameEventManager.instance.inventoryEvents.InventoryUpdated();

			return true;
		}

		/// <summary>
		/// Is there an instance of the item in the inventory?
		/// </summary>
		public bool InventoryHoldsItem(InventoryItem item)
		{
			for (int i = 0; i < slots.Length; i++)
			{
				if (object.ReferenceEquals(slots[i].item, item))
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
			return slots[slot].item;
		}

		/// <summary>
		/// Get the number of items in the given slot.
		/// </summary>
		public int GetAmountInSlot(int slot)
		{
			return slots[slot].amount;
		}

		/// <summary>
		/// Remove a number of items from the given slot. Will never remove more
		/// that there are.
		/// </summary>
		public void RemoveFromSlot(int slot, int amount)
		{
			slots[slot].amount -= amount;
			if (slots[slot].amount <= 0)
			{
				slots[slot].amount = 0;
				slots[slot].item = null;
			}

			GameEventManager.instance.inventoryEvents.InventoryUpdated();
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
			if (slots[slot].item != null)
			{
				return AddToFirstEmptySlot(item, amount); ;
			}

			int i = FindStack(item);
			if (i >= 0)
			{
				slot = i;
			}

			slots[slot].item = item;
			slots[slot].amount += amount;

			GameEventManager.instance.inventoryEvents.InventoryUpdated();

			return true;
		}


		// PRIVATE

		private void Awake()
		{
			slots = new InventorySlot[inventorySize];

#if UNITY_EDITOR
			if (loadInventoryFromSaveData)
			{
				// Load Inventory From Save Data
			}
#endif

		}

		/// <summary>
		/// Find an empty slot.
		/// </summary>
		/// <returns>-1 if all slots are full.</returns>
		private int FindEmptySlot()
		{
			for (int i = 0; i < slots.Length; i++)
			{
				if (slots[i].item == null)
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
				if (object.ReferenceEquals(slots[i].item, item))
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
			public int number;
		}

		// object ISaveable.CaptureState()
		// {
		// 	var slotStrings = new InventorySlotRecord[inventorySize];
		// 	for (int i = 0; i < inventorySize; i++)
		// 	{
		// 		if (slots[i].item != null)
		// 		{
		// 			slotStrings[i].itemID = slots[i].item.GetItemID();
		// 			slotStrings[i].number = slots[i].number;
		// 		}
		// 	}
		// 	return slotStrings;
		// }

		// void ISaveable.RestoreState(object state)
		// {
		// 	var slotStrings = (InventorySlotRecord[])state;
		// 	for (int i = 0; i < inventorySize; i++)
		// 	{
		// 		slots[i].item = InventoryItem.GetFromID(slotStrings[i].itemID);
		// 		slots[i].number = slotStrings[i].number;
		// 	}

		// 	GameEventManager.instance.inventoryEvents.InventoryUpdated();
		// }
	}
}
