using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Inventory;
using TMPro;

namespace Arcy.UI
{
	public class InventoryUiBtn : MenuBtn
	{
		[SerializeField] private bool _slotIsPopulated;
		[SerializeField] private InventoryItem item;
		[SerializeField] private TMP_Text amountTMP;

#if UNITY_EDITOR
		private void OnValidate()
		{
			amountTMP ??= GetComponentInChildren<TMP_Text>();
		}
#endif

		public void OnBtnSpawn(InventoryItem itemToShow = null)
		{
			_slotIsPopulated = item ? true : false;
			isInteractible = _slotIsPopulated;
		}
	}
}
