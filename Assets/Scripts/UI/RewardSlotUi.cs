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
		[SerializeField] public Image icon;
		[SerializeField] public TMPro.TMP_Text amountTMP;

		public bool currentlyInUse;
		public Inventory.InventoryItem item;

		private float startPosX;
		private float outsidePos = -420;


#if UNITY_EDITOR
		private void OnValidate()
		{
			if (amountTMP == null && icon != null)
				amountTMP = icon.GetComponentInChildren<TMPro.TMP_Text>();
		}
#endif

		private void Start()
		{
			startPosX = this.transform.position.x;
			// CanvasGroup cvGroup = TryGetComponent<CanvasGroup>(out CanvasGroup hit) ? hit : null;

			// Move the slots out of view;
			transform.DOMoveX(outsidePos, 1f, true);
		}

		public void SetUp(Inventory.InventoryItem item, int amount = 1)
		{
			icon = item.inventoryIcon;
			amountTMP.text = amount.ToString();
		}

		private void MoveUiSlotIn(Transform localTransform, CanvasGroup cvGroup)
		{
			Sequence sequence = DOTween.Sequence();

			sequence.Append(localTransform.DOMoveX(startPosX, 0.5f).SetEase(Ease.OutCubic)) // move in
			.Join(cvGroup.DOFade(1, 0.25f)) // fade in
			.OnComplete(() => MoveUiSlotOut(localTransform, cvGroup));
		}

		private void MoveUiSlotOut(Transform localTransform, CanvasGroup cvGroup)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.AppendInterval(1f) // short delay
			.Append(localTransform.DOMoveX(-420, 1f).SetEase(Ease.InCubic)) // move out
			.Join(cvGroup.DOFade(0, 1f)); // fade out
		}

		private void ItemResumed()
		{
			//DOTween.Kill();
		}
	}
}
