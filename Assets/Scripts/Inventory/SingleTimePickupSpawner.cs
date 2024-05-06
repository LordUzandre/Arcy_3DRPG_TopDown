using System.Collections;
using System.Collections.Generic;
using Arcy.Interaction;
using Arcy.Saving;
using UnityEngine;

namespace Arcy.Inventory
{
	public class SingleTimePickupSpawner : MonoBehaviour, ISaveableEntity
	{
		[SerializeField] private bool _useSaveData = true;

		public void LoadData(SaveData loadData)
		{

#if UNITY_EDITOR
			if (!_useSaveData)
			{
				return;
			}
#endif

			foreach (Transform child in transform)
			{
				if (child.TryGetComponent<Pickup>(out Pickup pickup))
				{
					loadData.pickupsCollected.TryGetValue(pickup.guid, out pickup.collected);

					if (pickup.collected)
					{
						Destroy(child.gameObject);
					}

					continue;
				}
				if (child.TryGetComponent<Chest>(out Chest chest))
				{
					loadData.pickupsCollected.TryGetValue(chest.guid, out bool isOpened);

					// If TryGetValue succeeds:
					if (isOpened)
						chest.isInteractible = false;
					else
						chest.isInteractible = true;

					chest.SetStartState(isOpened);

					continue;
				}
			}
		}

		public void SaveData(SaveData saveData)
		{

#if UNITY_EDITOR
			if (!_useSaveData)
			{
				return;
			}
#endif

			foreach (Transform child in transform)
			{
				if (child.TryGetComponent<Pickup>(out Pickup pickup) && pickup.collected)
				{
					if (saveData.pickupsCollected.ContainsKey(pickup.guid))
					{
						saveData.pickupsCollected.Remove(pickup.guid);
					}

					saveData.pickupsCollected.Add(pickup.guid, pickup.collected);

					continue;
				}

				if (child.TryGetComponent<Chest>(out Chest chest))
				{
					if (saveData.pickupsCollected.ContainsKey(chest.guid))
					{
						saveData.pickupsCollected.Remove(chest.guid);
					}

					saveData.pickupsCollected.Add(chest.guid, !chest.isInteractible);

					continue;
				}
			}
		}

	}
}
