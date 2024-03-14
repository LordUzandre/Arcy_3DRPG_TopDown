using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Management;
using UnityEngine;

namespace Arcy.Inventory
{
	public class InventoryGem : MonoBehaviour
	{
		[SerializeField] InventoryItem item;

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				Destroy(gameObject);
				GameEventManager.instance.inventoryEvents.InventoryItemAdded(item);
			}
		}
	}
}
