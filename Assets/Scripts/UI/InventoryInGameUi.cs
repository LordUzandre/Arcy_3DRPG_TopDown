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

		private void OnEnable()
		{
			GameManager.instance.gameEventManager.inventoryEvents.onInventoryItemAdded += ItemsAdded;
		}

		private void OnDisable()
		{
			GameManager.instance.gameEventManager.inventoryEvents.onInventoryItemAdded -= ItemsAdded;
		}

		public void ItemsAdded(InventoryItem item, int amountAdded)
		{
			// Check if any of the slots currently contain the item in question
			foreach (RewardSlotUi slot in _slotsCurrentlyInUse)
			{
				if (slot.item.GetDisplayName() == item.GetDisplayName())
				{
					Debug.Log("add number to already populated slot");

					// Put at the top of the list
					//slot.transform.SetSiblingIndex(0);

					slot.amountAddedInt += amountAdded;

					slot.MoveUiSlotOut(slot.transform, slot.GetComponent<CanvasGroup>());
					return;
				}
			}

			// Otherwise occupy the first unused slot
			for (int i = 0; i < _slots.Length; i++)
			{
				if (!_slots[i].currentlyInUse)
				{
					// TODO - add code
					Debug.Log("slot.item.name = " + _slots[i].item.GetGuid() + ", item.name = " + item.GetGuid());
					_slots[i].Initialize(item, amountAdded);
					return;
				}
			}

			// If all are occupied, use the "oldest" one
		}
	}
}
