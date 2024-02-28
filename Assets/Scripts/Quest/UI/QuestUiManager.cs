using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace Arcy.Quests
{
	public class QuestUiManager : MonoBehaviour
	{
		[Header("Quest Manager")]
		[SerializeField] private QuestManager _questManager;
		[Header("Left Window")]
		[SerializeField] private GameObject _ongoingQuestParent;
		[SerializeField] private GameObject _finishedQuestParent;
		[Header("Quest Lists")]
		[SerializeField] private List<QuestObject> _ongoingQuests = new List<QuestObject>();
		[SerializeField] private List<QuestObject> _finishedQuests = new List<QuestObject>();
		[Header("Quest UI Btn Prefab")]
		[SerializeField] private GameObject _questUiBtnPrefab;
		[Header("Right Window Panel")]
		[SerializeField] private QuestWindow _questWindow;

#if UNITY_EDITOR
		private void OnValidate()
		{
			// SetRectSize(_ongoingQuestParent.gameObject.GetComponent<RectTransform>(), _ongoingQuests);
			// SetRectSize(_finishedQuestParent.gameObject.GetComponent<RectTransform>(), _finishedQuests);
		}
#endif

		private void Start()
		{
			CheckComponents();
		}

		private void CheckComponents()
		{
			if (_ongoingQuestParent == null)
			{
				_ongoingQuestParent = transform.Find("QuestUiParent").gameObject;
			}

			if (_finishedQuestParent == null)
			{
				_finishedQuestParent = transform.Find("FinishedUiParent").gameObject;
			}

			// Add the ongoingQuests and spawn all the finished quests in QuestManager from QuestManager
			if (_ongoingQuestParent != null)
			{
				PopulateLists(_ongoingQuests, _questManager.ongoingQuests, _ongoingQuestParent.transform);
			}

			if (_finishedQuestParent != null)
			{
				PopulateLists(_finishedQuests, _questManager.finishedQuests, _finishedQuestParent.transform);
			}
		}

		private void PopulateLists(List<QuestObject> questListToPopulate, List<QuestObject> ogList, Transform parentTransform)
		{
			// Destroy any placeholder quest in the list
			foreach (Transform child in parentTransform)
			{
				Destroy(child.gameObject);
			}

			questListToPopulate.Clear();
			questListToPopulate.AddRange(ogList);
			questListToPopulate = ogList;

			for (int i = 0; i < questListToPopulate.Count; i++)
			{
				GameObject questBtnObj = Instantiate(_questUiBtnPrefab, parentTransform);
				QuestUiBtn questBtn = questBtnObj.GetComponent<QuestUiBtn>();
				questBtn.NewBtnSpawned(ogList[i]);
			}

			// Set the size of _ongoingQuestsParent.rect based on number of quests?
			SetRectSize(parentTransform.gameObject.GetComponent<RectTransform>(), questListToPopulate);
		}

		private void SetRectSize(RectTransform rect, List<QuestObject> quests)
		{
			rect.sizeDelta = new Vector2(rect.sizeDelta.x, 120 * quests.Count);
		}
	}
}
