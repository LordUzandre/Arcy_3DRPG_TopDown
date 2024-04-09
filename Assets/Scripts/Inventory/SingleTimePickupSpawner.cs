using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Arcy.Inventory
{
	public class SingleTimePickupSpawner : MonoBehaviour
	{
		private void Start()
		{
			CheckLoadData();
		}

		private void CheckLoadData()
		{
			foreach (Transform child in transform)
			{
				if (child.TryGetComponent<Pickup>(out Pickup pickup) && pickup.collected)
				{
					Destroy(child.gameObject);
				}
			}
		}
	}
}
