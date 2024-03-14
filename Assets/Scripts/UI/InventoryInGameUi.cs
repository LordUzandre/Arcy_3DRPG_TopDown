using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Inventory;
using Arcy.Management;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.UI
{
	public class InventoryInGameUi : MonoBehaviour
	{
		[SerializeField] private RewardSlotUi[] _slots;
		[Space]
		[SerializeField] private List<RewardSlotUi> _slotsCurrentlyInUse;

		private RectTransform _rect;
		private float _tmpRect;

		private bool _sameItem;

		private void OnEnable()
		{
			GameEventManager.instance.inventoryEvents.onInventoryItemAdded += ItemsAdded;
		}

		private void OnDisable()
		{
			GameEventManager.instance.inventoryEvents.onInventoryItemAdded -= ItemsAdded;
		}

		public void ItemsAdded(InventoryItem item)
		{
			// Check if any of the slots currently contain the item in question
			foreach (RewardSlotUi slot in _slotsCurrentlyInUse)
			{
				if (slot.item == item)
				{
					// TODO - add code
					return;
				}
			}

			// Otherwise occupy the first unused slot
			foreach (RewardSlotUi slot in _slots)
			{
				if (!slot.currentlyInUse)
				{
					// TODO - add code
					return;
				}
			}

			// If all are occupied, use the "oldest" one
		}
	}
}
