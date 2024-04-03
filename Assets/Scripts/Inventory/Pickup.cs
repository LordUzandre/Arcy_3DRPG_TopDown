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

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				if (item != null)
					GameEventManager.instance.inventoryEvents.InventoryItemAdded(item);

				Destroy(gameObject);
			}
		}
	}
}
