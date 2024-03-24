using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Arcy.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class RewardSlotUi : MonoBehaviour
	{
		// public
		[Header("Item currently in slot")]
		[SerializeField] public Inventory.InventoryItem item;

		[Header("Components")]
		[SerializeField] public Image icon;
		[SerializeField] public TMPro.TMP_Text amountTMP;

		public bool currentlyInUse;
		[HideInInspector] public int amountAddedInt = 0;

		// private:
		// private Sequence sequence;
		private float insidePos = 0;
		private float outsidePos = -420;
		private float _timer = 0;


#if UNITY_EDITOR
		private void OnValidate()
		{
			if (icon == null)
			{
				icon = transform.Find("RewardImage").GetComponent<Image>();
			}

			if (amountTMP == null && icon != null)
				amountTMP = icon.GetComponentInChildren<TMPro.TMP_Text>();
		}
#endif

		private void Start()
		{
			insidePos = this.transform.position.x;

			// Move the slots out of view;
			transform.DOMoveX(outsidePos, 1f, true);
		}

		public void Initialize(Inventory.InventoryItem item, int amount)
		{
			// Components:
			currentlyInUse = true;
			amountAddedInt = amount;
			icon.sprite = item.GetIcon();
			amountTMP.text = amountAddedInt.ToString();
			_timer = 0;

			MoveUiSlotIn(this.transform, GetComponent<CanvasGroup>());
		}

		#region UI-animations
		private void MoveUiSlotIn(Transform localTransform, CanvasGroup cvGroup)
		{
			cvGroup.alpha = 0;
			Sequence sequence = DOTween.Sequence();

			sequence.Append(localTransform.DOMoveX(insidePos, 0.2f).SetEase(Ease.OutCubic)) // move in
				.Join(cvGroup.DOFade(1, 0.25f)) // fade in
				.OnComplete(() => MoveUiSlotOut(localTransform, cvGroup));
		}

		public void MoveUiSlotOut(Transform localTransform, CanvasGroup cvGroup)
		{
			Sequence sequence = DOTween.Sequence();

			// Chack starting position
			if (this.transform.position.x != insidePos)
			{
				sequence.Append(localTransform.DOMoveX(insidePos, 0.1f).SetEase(Ease.OutCubic)); // move in
			}

			sequence.AppendInterval(1f) // short delay
			.Append(localTransform.DOMoveX(-420, 1f).SetEase(Ease.InCubic)) // move out
			.Join(cvGroup.DOFade(0, 1f)) // fade out
			.OnComplete(() => ResetSlot());
		}
		#endregion

		// After the Slot have moved out of view
		private void ResetSlot()
		{
			currentlyInUse = false;
			item = null;
			amountAddedInt = 1;
		}

		private void Update()
		{
			if (_timer < 10)
			{
				_timer += Time.deltaTime;
			}
			else
			{
				_timer = 0;
			}
		}
	}
}
