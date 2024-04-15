using System.Collections;
using System.Collections.Generic;
using Arcy.Saving;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Arcy.Inventory
{
	public class SingleTimePickupSpawner : MonoBehaviour, ISaveableEntity
	{
		public void LoadData(SaveData data)
		{
			foreach (Transform child in transform)
			{
				if (child.TryGetComponent<Pickup>(out Pickup pickup))
				{
					data.coinsCollected.TryGetValue(pickup.guid, out pickup.collected);

					if (pickup.collected)
					{
						Destroy(child.gameObject);
					}
				}
			}
		}

		public void SaveData(SaveData data)
		{
			foreach (Transform child in transform)
			{
				if (child.TryGetComponent<Pickup>(out Pickup pickup) && pickup.collected)
				{
					if (data.coinsCollected.ContainsKey(pickup.guid))
					{
						data.coinsCollected.Remove(pickup.guid);
					}

					data.coinsCollected.Add(pickup.guid, pickup.collected);
				}
			}

			SerializableDictionary<string, bool> serializedDictionary = new SerializableDictionary<string, bool>();
		}
	}
}
