using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Arcy.Inventory
{
	[System.Serializable]
	public class InventorySlot
	{
		[SerializeField] private InventoryItem _item;
		[SerializeField] private int _amount;

		public InventorySlot(InventoryItem item, int amount)
		{
			_item = item;
			_amount = amount;
		}

		public bool IsPopulated()
		{
			return GetAmount() > 0 && GetItem() != null;
		}

		public InventoryItem GetItem()
		{
			return _item;
		}

		public void SetItem(InventoryItem newItem)
		{
			_item = newItem;
		}

		public int GetAmount()
		{
			return _amount;
		}

		public void SetAmount(int amount)
		{
			_amount = amount;
		}

		public void AddtoAmount(int amountToAdd)
		{
			_amount += amountToAdd;
		}

		public void SubstractFromSlot(int amountToRemove)
		{
			_amount -= amountToRemove;
		}
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(InventorySlot))]
	public class InventorySlotEditor : PropertyDrawer
	{
		// private SerializedProperty _item;
		private SerializedProperty _itemProperty;
		private SerializedProperty _amountProperty;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			// Get the serialized property of the InventoryItem
			_itemProperty = property.FindPropertyRelative("_item");
			_amountProperty = property.FindPropertyRelative("_amount");

			EditorGUILayout.PropertyField(_itemProperty);

			EditorGUI.EndProperty();

			// Access the InventoryItem instance
			InventoryItem inventoryItem = (InventoryItem)_itemProperty.objectReferenceValue;

			if (inventoryItem != null)
			{
				bool itemIsStackable = inventoryItem.stackable;

				if (itemIsStackable)
				{
					EditorGUILayout.PropertyField(_amountProperty);
				}

				// EditorGUI.BeginChangeCheck();
				// isStackable = EditorGUILayout.Toggle("Is Stackable", isStackable);
				// if (EditorGUI.EndChangeCheck())
				// {
				// 	// Apply changes
				// 	inventoryItem.stackable = isStackable;
				// 	EditorUtility.SetDirty(inventoryItem);
				// }
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}
	}
#endif
}
