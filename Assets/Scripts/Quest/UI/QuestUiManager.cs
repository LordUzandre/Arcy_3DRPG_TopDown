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
		[Header("Quest Lists")]
		[SerializeField] private List<QuestObject> _ongoingQuests = new List<QuestObject>();
		[SerializeField] private List<QuestObject> _finishedQuests = new List<QuestObject>();
		[Header("Quest UI Btn Prefab")]
		[SerializeField] private GameObject _questUiBtnPrefab;
		[Header("Right Window Panel")]
		[SerializeField] private QuestWindow _questWindow;
		//[SerializeField] private Transform ongoingListParent;

		private void Start()
		{
			// Destroy any placeholder quest in the list
			foreach (Transform child in _ongoingQuestParent.transform)
			{
				Destroy(child.gameObject);
			}

			_ongoingQuests.Clear();
			_finishedQuests.Clear();

			// Add the ongoingQuests from QuestManager
			foreach (QuestObject ongoingQuest in _questManager.ongoingQuests)
			{
				GameObject questBtn = Instantiate(_questUiBtnPrefab, _ongoingQuestParent.transform);
				_ongoingQuests.Add(questBtn.GetComponent<QuestObject>());
			}

			// Set the size of _ongoingQuestsParent.rect based on number of quests?

			// TODO: Also spawn all the finished quests in QuestManager
		}
	}
}
