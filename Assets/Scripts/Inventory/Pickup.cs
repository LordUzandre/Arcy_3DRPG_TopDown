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
		[Header("SaveData")]
		[SerializeField] public string guid;
		[SerializeField] public bool collected;

		private void OnValidate()
		{
			guid ??= System.Guid.NewGuid().ToString();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player" && item != null)
			{
				collected = true;
				GameManager.instance.gameEventManager.inventoryEvents.InventoryItemAdded(item);
				Destroy(gameObject);
			}
		}
	}
}
