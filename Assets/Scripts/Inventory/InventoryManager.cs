using System.Collections;
using System.Collections.Generic;
using Arcy.Management;
using Arcy.Saving;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Arcy.Inventory
{
	public class InventoryManager : MonoBehaviour, ISaveableEntity
	{
		[Header("Config")]
		[SerializeField] private bool _debugging;
		[SerializeField] private bool _useSaveData = true;
		[SerializeField] private StarterInventory _starterInventoryPrefab;

		[Space]
		[SerializeField] public static int InventorySize = 16;
		[SerializeField] public static int GoldCoins = 0;
		[SerializeField] public static int Experience = 0;
		[SerializeField] public static InventorySlot[] EquipmentSlots;
		[SerializeField] public static InventorySlot[] ConsumableSlots;

		// For use in Editor.
#if UNITY_EDITOR
		[HideInInspector] public int InEditorGoldCoins = 0;
		[HideInInspector] public int InEditorExpPoints = 0;
		[HideInInspector] public InventorySlot[] InEditorSlots;
		[HideInInspector] public InventorySlot[] InEditorEquipment;

		private void Update()
		{
			InEditorGoldCoins = GoldCoins;
			InEditorExpPoints = Experience;
			InEditorEquipment = EquipmentSlots;
			InEditorSlots = ConsumableSlots;
		}
#endif

		/*
		MARK: PUBLIC
		------------------------------------------------------------------------------
		*/

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
			foreach (InventorySlot slot in ConsumableSlots)
			{
				if (slot.GetAmount() < 1)
				{
					count++;
				}
			}
			return count;
		}

		/// <summary>
		/// Is there an instance of the item in the inventory?
		/// </summary>
		public bool HasItem(InventoryItem item)
		{
			for (int i = 0; i < ConsumableSlots.Length; i++)
			{
				if (object.ReferenceEquals(ConsumableSlots[i].GetItem(), item))
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

			ConsumableSlots[i].SetItem(item);
			ConsumableSlots[i].AddtoAmount(amount);

			GameManager.instance.gameEventManager.inventoryEvents.InventoryUpdated();

			return true;
		}

		/// <summary>
		/// Is there an instance of the item in the inventory?
		/// </summary>
		public bool InventoryHoldsItem(InventoryItem item)
		{
			for (int i = 0; i < ConsumableSlots.Length; i++)
			{
				if (object.ReferenceEquals(ConsumableSlots[i].GetItem(), item))
				{
					return true;
				}
			}

			return false;
		}

		public void RemoveFromSlot(int slot, int amount)
		{
			ConsumableSlots[slot].SubstractFromSlot(amount);
			if (ConsumableSlots[slot].GetAmount() <= 0)
			{
				ConsumableSlots[slot].SetAmount(0);
				ConsumableSlots[slot].SetItem(null);
			}

			if (_debugging) Debug.Log("InventoryManager: " + amount + " " + ConsumableSlots[slot].GetItem().name + " removed from inventory.");
			GameManager.instance.gameEventManager.inventoryEvents.InventoryUpdated();
		}

		public bool AddItemToSlot(int slot, InventoryItem item, int amount)
		{
			if (ConsumableSlots[slot].GetItem() != item)
			{
				return AddToFirstEmptySlot(item, amount);
			}

			int i = FindStack(item);

			if (i >= 0) { slot = i; }

			ConsumableSlots[slot].SetItem(item);
			ConsumableSlots[slot].AddtoAmount(amount);

			// debugging
			if (_debugging) Debug.Log("InventoryManager: " + amount + " " + item.name + " added to inventory-slot number: " + slot);

			GameManager.instance.gameEventManager.inventoryEvents.InventoryUpdated();

			return true;
		}

		/*
		MARK: GETTERS
		------------------------------------------------------------------------------
		*/

		public int GetInventorySize() { return ConsumableSlots.Length; }

		public InventoryItem GetItemInSlot(int slot) { return ConsumableSlots[slot].GetItem(); }

		public int GetAmountInSlot(int slot) { return ConsumableSlots[slot].GetAmount(); }

		/*
		MARK: PRIVATE
		------------------------------------------------------------------------------
		*/

		private InventorySlot[] LoadFromStarterInventory()
		{
			if (_starterInventoryPrefab != null)
			{
				// Get the original array
				InventorySlot[] originalArray = _starterInventoryPrefab.LoadStarterInventory();
				InventorySlot[] clonedArray = new InventorySlot[originalArray.Length];

				for (int i = 0; i < InventorySize; i++)
				{
					InventoryItem newItem = originalArray[i].GetItem();
					// Create a new InventorySlot instance with the cloned InventoryItem and amount
					clonedArray[i] = new InventorySlot(newItem, originalArray[i].GetAmount());
				}

				if (_debugging) Debug.Log("InventoryManager: Loaded from Starter Inventory");

				return clonedArray;
			}
			else
			{
				Debug.LogError("InventoryManager: unable to acquire items from starterInventory");
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
			for (int i = 0; i < ConsumableSlots.Length; i++)
			{
				if (ConsumableSlots[i].GetItem() == null)
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
				if (_debugging) Debug.Log("First Empty Slot Found: " + i);
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

			for (int i = 0; i < ConsumableSlots.Length; i++)
			{
				if (object.ReferenceEquals(ConsumableSlots[i].GetItem(), item))
				{
					return i;
				}
			}

			return -1;
		}

		/*
		MARK: Save
		------------------------------------------------------------------------------
		*/


		public void SaveData(SaveData saveData)
		{
#if UNITY_EDITOR
			if (!SaveDataManager.GlobalOverrideSaveData) { if (!_useSaveData) { return; } }
			if (ConsumableSlots.Length == 0) return;
#endif

			saveData.inventory.Clear();
			saveData.inventorySize = InventorySize;

			List<string> stringListToBeSaved = new List<string>();

			foreach (InventorySlot slot in ConsumableSlots)
			{
				if (slot.IsPopulated())
				{
					// int guid = slot.GetItem().GetGuid();
					// saveData.inventory.Add(guid, slot.GetAmount());

					string serializedData = JsonUtility.ToJson(slot);
					stringListToBeSaved.Add(serializedData);
				}
			}

			saveData.inventoryString = stringListToBeSaved.ToArray();

		}

		/*
		MARK: Load
		------------------------------------------------------------------------------
		*/

		public void LoadData(SaveData loadData)
		{
			if (loadData.inventory.Count < 1)
			{
				ConsumableSlots = LoadFromStarterInventory();
				return;
			}

#if UNITY_EDITOR
			if (!SaveDataManager.GlobalOverrideSaveData) { if (!_useSaveData) { return; } }
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
					if (_debugging) Debug.LogError("Unable to access item: " + itemID.Key);
					continue;
				}

				consumablesToBeAdded.Add(newSlot);
			}

			if (consumablesToBeAdded.Count < InventorySize)
			{
				int remainingSlots = InventorySize - consumablesToBeAdded.Count;
				for (int i = 0; i < remainingSlots; i++)
				{
					consumablesToBeAdded.Add(null);
				}
			}

			ConsumableSlots = consumablesToBeAdded.ToArray();

		}
	}

	/*
	MARK: EDITOR
	------------------------------------------------------------------------------
	*/

#if UNITY_EDITOR
	[CustomEditor(typeof(InventoryManager))]
	public class InventoryEditor : Editor
	{
		private SerializedProperty myArray;

		public override void OnInspectorGUI()
		{
			InventoryManager myTarget = (InventoryManager)target;

			base.OnInspectorGUI();

			EditorGUILayout.Space();
			EditorGUI.indentLevel += 2;
			EditorGUILayout.LabelField("In-Editor Inventory", EditorStyles.whiteLabel);
			EditorGUI.indentLevel += 2;
			EditorGUILayout.BeginHorizontal(new GUIStyle());
			EditorGUILayout.LabelField("Item (GUID): ");
			EditorGUILayout.LabelField("Amount: ");
			EditorGUILayout.EndHorizontal();

			if (myTarget.InEditorSlots.Length > 0)
			{
				EditorGUILayout.Space();
				foreach (InventorySlot slot in myTarget.InEditorSlots)
				{
					if (slot.GetAmount() <= 0) return;
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(slot.GetItem().GetItemName() + " (" + slot.GetItem().GetGuid().ToString() + ")");
					EditorGUILayout.LabelField(slot.GetAmount().ToString());
					EditorGUILayout.EndHorizontal();
				}

			}
			else if (myTarget.InEditorSlots.Length <= 0)
			{
				// Try to get the slots from the save-file
				string fullPath = System.IO.Path.Combine(Application.persistentDataPath, "save.game");
				string dataToLoad = "";
				SaveData loadedData = null;

				if (File.Exists(fullPath))
				{
					try
					{
						using (FileStream stream = new FileStream(fullPath, FileMode.Open))
						{
							using (StreamReader reader = new StreamReader(stream))
							{
								dataToLoad = reader.ReadToEnd();
							}
						}

						loadedData = JsonUtility.FromJson<SaveData>(dataToLoad);
						Debug.Log(dataToLoad);

						List<string> Items = new List<string>();
						List<int> Values = new List<int>();


						if (loadedData.inventory.Count < 1) return;

						foreach (KeyValuePair<int, int> itemID in loadedData.inventory)
						{
							if (itemID.Key == 0 || itemID.Value <= 0)
							{
								break;
							}

							Items.Add(InventoryItem.GetFromID(itemID.Key).GetItemName());
							Values.Add(itemID.Value);

						}
					}
					catch (System.Exception)
					{

						Debug.LogError("Inventory: Unable to retrieve inventory from save files");
					}
				}
			}

			EditorGUI.indentLevel = 0;
		}
	}
#endif

}
