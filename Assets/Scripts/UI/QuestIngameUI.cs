using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Arcy namespaces
using Arcy.Management;
using Arcy.Quests;
// Other libraries
using TMPro;
using DG.Tweening;

namespace Arcy.UI
{
	public class QuestIngameUI : MonoBehaviour
	{
		[Header("TextMeshPros")]
		// private:
		[SerializeField] private TMP_Text _questUpdatedTMP;
		[SerializeField] private TMP_Text _questNameTMP;
		[SerializeField] private TMP_Text _pressJToLearnMoreTMP;

		[Header("Reward Slots")]
		[SerializeField] private Transform _inventorySlotsParent;
		[SerializeField] private RewardSlotUi[] _rewardSlots;

		Vector3 startPosition;

#if UNITY_EDITOR
		private void OnValidate()
		{
			// Auto-populate _rewardSlot
			if (_rewardSlots.Length == 0 && _inventorySlotsParent != null)
			{
				List<RewardSlotUi> rewardSlots = new List<RewardSlotUi>();
				foreach (Transform child in _inventorySlotsParent)
				{
					RewardSlotUi rewardSlot = TryGetComponent<RewardSlotUi>(out RewardSlotUi hit) ? hit : null;
					rewardSlots.Add(rewardSlot);
				}

				_rewardSlots = rewardSlots.ToArray();
			}
		}
#endif

		private void Start()
		{
			startPosition = this.transform.position;

			Sequence sequence = DOTween.Sequence();
			sequence.AppendInterval(1f) // short delay
			.Append(transform.DOMoveX(-420, 1f, false).SetEase(Ease.OutCubic)); // move out
		}

		private void OnEnable()
		{
			GameEventManager.instance.questEvents.onQuestStateChange += QuestUpdated;
		}

		private void OnDisable()
		{
			GameEventManager.instance.questEvents.onQuestStateChange -= QuestUpdated;
		}

		public void QuestUpdated(Quest quest)
		{
			// TODO - Check wether the quest is new, updated or finished
			_questNameTMP.text = quest.info.displayName;
		}

		private void SetupRewardSlots()
		{

		}
	}
}
