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
		[SerializeField] public bool collected;

		// TODO - implement save/load
		private void Awake()
		{
			CheckCollected();
		}

		private void CheckCollected()
		{

		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player" && item != null)
			{
				collected = true;
				GameEventManager.instance.inventoryEvents.InventoryItemAdded(item);
				Destroy(gameObject);
			}
		}
	}
}
