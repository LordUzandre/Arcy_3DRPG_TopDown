using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Inventory;
using TMPro;

namespace Arcy.UI
{
	public class GoldCoinUi : MonoBehaviour
	{
		[SerializeField] private TMP_Text _goldCoinTMP;
		[SerializeField] private InventoryItem _goldCoinItem;
		private WaitForFixedUpdate _delay = new WaitForFixedUpdate();
		private int _goldCoinGuid = 0;

		private int _currentCoin = 0;
		private int _coinTotal = 0;

#if UNITY_EDITOR
		private void OnValidate()
		{
			CheckComponents();
		}
#endif

		private void CheckComponents()
		{
			_goldCoinTMP ??= GetComponentInChildren<TMP_Text>();
			// TODO - Also check goldcoin
		}

		private void OnEnable()
		{
			CheckComponents();
			_coinTotal = InventoryManager.GoldCoins;
			_goldCoinGuid = _goldCoinItem.GetGuid();
			_goldCoinTMP.text = _coinTotal.ToString();

			Management.GameManager.instance.gameEventManager.inventoryEvents.onInventoryItemAdded += InventoryUpdated;
			// Management.GameManager.instance.gameEventManager.inventoryEvents.onInventoryItemUsed += InventoryUpdated;
			Management.GameManager.instance.gameEventManager.inventoryEvents.onInventoryItemRemoved += InventoryUpdated;
		}

		private void OnDisable()
		{
			Management.GameManager.instance.gameEventManager.inventoryEvents.onInventoryItemAdded -= InventoryUpdated;
			// Management.GameManager.instance.gameEventManager.inventoryEvents.onInventoryItemUsed -= InventoryUpdated;
			Management.GameManager.instance.gameEventManager.inventoryEvents.onInventoryItemRemoved -= InventoryUpdated;
		}

		private void InventoryUpdated(InventoryItem item, int amount = 0)
		{
			if (item.GetGuid() == _goldCoinGuid && amount != 0)
			{
				StartCoroutine(myRoutine(amount));
			}
		}

		IEnumerator myRoutine(int coinDifference)
		{
			_coinTotal += coinDifference;

			yield return new WaitForSeconds(0.5f);

			while (_currentCoin != _coinTotal)
			{
				if (_currentCoin < _coinTotal)
				{
					_currentCoin++;
				}
				else if (_currentCoin > _coinTotal)
				{
					_currentCoin--;
				}

				_goldCoinTMP.text = _currentCoin.ToString();

				yield return _delay;
			}
		}

	}
}