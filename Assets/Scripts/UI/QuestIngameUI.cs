using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Quests;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Arcy.Management;

namespace Arcy.UI
{
	public class QuestIngameUI : MonoBehaviour
	{
		// Private:
		[Header("TextMeshPro-elements")]
		[SerializeField] private TMP_Text _questNameTMP;
		[SerializeField] private TMP_Text _questUpdateTMP;
		[Header("RewardSlots")]
		[SerializeField] private GameObject _rewardSlot01;
		[SerializeField] private GameObject _rewardSlot02;
		[SerializeField] private GameObject _rewardSlot03;

		[Header("Image-elements")]
		[SerializeField] private Image _circleBg;
		[SerializeField] private GameObject _separators;

		Sequence fadeInSequence;

		private void Start()
		{
			fadeInSequence = DOTween.Sequence();
		}

		private void OnEnable()
		{
			GameManager.instance.gameEventManager.questEvents.onQuestStateChange += QuestUpdated;
		}

		private void OnDisable()
		{
			GameManager.instance.gameEventManager.questEvents.onQuestStateChange -= QuestUpdated;
		}

		public void QuestUpdated(Quest quest)
		{
			// TODO - Check wether the quest is new, updated or finished
			FadeIn(quest);
		}

		private void FadeIn(Quest quest)
		{
			// Set all necessary elements to zero
			_separators.SetActive(true);
			CanvasGroup separatorsCvGroup = _separators.TryGetComponent<CanvasGroup>(out CanvasGroup hit) ? hit : null;
			separatorsCvGroup.alpha = 0;
			CanvasGroup circleCvGroup = _circleBg.gameObject.TryGetComponent<CanvasGroup>(out CanvasGroup cvGroup) ? hit : cvGroup;
			circleCvGroup.alpha = 0;

			// Sequence
			_separators.transform.DOScaleX(0, 0);
			fadeInSequence.Append(circleCvGroup.DOFade(1, 0.5f));
			fadeInSequence.PrependInterval(0.5f);
			fadeInSequence.Append(_separators.transform.DOScaleX(1, 0.25f))
			.Join(separatorsCvGroup.DOFade(1, 0.5f));
		}
	}
}
