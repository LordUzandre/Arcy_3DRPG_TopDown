using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Battle
{
	[RequireComponent(typeof(Button))]
	public class SelectionVisualBtn : MonoBehaviour
	{
		[SerializeField] private BattleCharacterBase _bgBase;
		private Button _btn;

		private void OnEnable()
		{
			_btn = GetComponent<Button>();
			_btn.onClick.AddListener(WhenBtnPressed);
		}

		// When the Btn should be clickable
		public void ActivateBtn(bool myBool)
		{
			_btn.interactable = myBool;
		}

		private void WhenBtnPressed()
		{
			BattleManager.instance.playerCombatManager.CastCombatAction(_bgBase);
		}
	}
}