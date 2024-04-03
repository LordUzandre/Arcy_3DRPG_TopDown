using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Quests;
using UnityEngine.Events;

namespace Arcy.UI
{
	public class QuestLogScrollingList : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private GameObject _contentParent;
		[Header("")]
		[SerializeField] private RectTransform _scrollRectTransform;
		[SerializeField] private RectTransform _contentRectTransform;
		[Header("Quest Btn Prefab")]
		[SerializeField] private GameObject _questLogBtnPrefab;

		private Dictionary<string, QuestLogBtn> _idToBtnMap = new Dictionary<string, QuestLogBtn>();

		public QuestLogBtn CreateBtnIfNotExists(Quest quest, UnityAction selectAction)
		{
			QuestLogBtn questLogBtn = null;

			if (!_idToBtnMap.ContainsKey(quest.infoSO.guid))
			{
				questLogBtn = InstantiateLogBtn(quest, selectAction);
			}
			else
			{
				questLogBtn = _idToBtnMap[quest.infoSO.guid];
			}

			return questLogBtn;
		}

		private QuestLogBtn InstantiateLogBtn(Quest quest, UnityAction selectAction)
		{
			// Create the button
			QuestLogBtn questLogBtn = Instantiate(_questLogBtnPrefab, _contentParent.transform).GetComponent<QuestLogBtn>();

			// Game object name in the hierarchy
			questLogBtn.gameObject.name = quest.infoSO.guid + "_button";

			// Initialize and set up function for when the button is selected
			RectTransform btnRectTransform = questLogBtn.GetComponent<RectTransform>();
			questLogBtn.Initialize(quest.infoSO.displayName, () =>
			{
				selectAction();
				UpdateScrolling(btnRectTransform);
			});

			// add to map to keep track of the new button
			_idToBtnMap[quest.infoSO.guid] = questLogBtn;

			return questLogBtn;
		}

		private void UpdateScrolling(RectTransform btnRectTransform)
		{
			// Calculate the min and max for the selected button
			float btnYMin = Mathf.Abs(btnRectTransform.anchoredPosition.y);
			float btnYMax = btnYMin + btnRectTransform.rect.height;

			// Calculate the min and max for the content area
			float contentYMin = _contentRectTransform.anchoredPosition.y;
			float contentYMax = contentYMin + _scrollRectTransform.rect.height;

			// Handle scrolling down
			if (btnYMax > contentYMax)
			{
				_contentRectTransform.anchoredPosition = new Vector2(_contentRectTransform.anchoredPosition.x, btnYMax - _scrollRectTransform.rect.height);
			}

			if (btnYMin < contentYMin)
			{
				_contentRectTransform.anchoredPosition = new Vector2(_contentRectTransform.anchoredPosition.x, btnYMin - _scrollRectTransform.rect.height);
			}
		}
	}
}
