using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Inventory;
using Arcy.Management;

namespace Arcy.UI
{
	public class InventoryUi : MonoBehaviour
	{
		// [Header("Inventory")]
		// [SerializeField] private InventoryManager _inventory;
		[Header("Consumables-components")]
		[SerializeField] private Transform _gridParent;
		[SerializeField] private GameObject _itemSlotPrefab;

		[SerializeField] private List<InventoryUiBtn> btnList;

		private void OnEnable()
		{
			GameManager.instance.gameStateManager.OnGameStateChanged += OnGameStateChanged;
		}

		private void OnDisable()
		{
			GameManager.instance.gameStateManager.OnGameStateChanged -= OnGameStateChanged;
		}

		private void OnGameStateChanged(GameState state)
		{
			if (state == GameState.Pause)
			{
				PopulateArray();
			}
		}

		private void CheckComponents()
		{

		}

		private void PopulateArray()
		{
			// Destroy any placeholders
			foreach (Transform child in _gridParent)
			{
				Destroy(child.gameObject);
			}

			btnList.Clear();
			btnList.RemoveRange(0, btnList.Count);

			for (int i = 0; i < InventoryManager.inventorySize; i++)
			{
				GameObject gmObject = Instantiate(_itemSlotPrefab, _gridParent);
				InventoryUiBtn ivBtn = gmObject.GetComponent<InventoryUiBtn>();
				btnList.Add(ivBtn);
				ivBtn.OnBtnSpawn();
			}
		}
	}
}
