using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Management;
using UnityEngine;

namespace Arcy.Inventory
{
	public class Pickup : MonoBehaviour
	{
		[SerializeField] InventoryItem item;
		[Header("SaveData (for pickup-spawner, not inventory)")]
		[SerializeField] public int guid = 0;
		[SerializeField] public bool collected = false;

		private void OnValidate()
		{
			if (guid == 0)
			{
				guid = Utils.GuidGenerator.guid();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player" && item != null)
			{
				collected = true;
				gameObject.SetActive(false);
				GameManager.instance.gameEventManager.inventoryEvents.InventoryItemAdded(item);
			}
		}
	}
}
