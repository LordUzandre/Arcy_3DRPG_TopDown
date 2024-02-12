using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Arcy.Quest
{
	public class QuestUiManager : MonoBehaviour
	{
		[Header("Quest Manager")]
		[SerializeField] private QuestManager _questManager;
		[Header("Quest Lists")]
		[SerializeField] private List<QuestObject> _ongoingQuests = new List<QuestObject>();
		[SerializeField] private List<QuestObject> _finishedQuests = new List<QuestObject>();
		[Header("Quest UI Btn Prefab")]
		[SerializeField] private GameObject _questUiBtnPrefab;
		[Header("TextMeshPro Components")]
		[SerializeField] private TMP_Text _questWindowTitle;
		[SerializeField] private TMP_Text _questWindowGiverText;
		[SerializeField] private TMP_Text _questWindowLocationText;
		[SerializeField] private TMP_Text _questWindowDescriptionText;
		[Header("Icons")]
		[SerializeField] private Image _questWindowGiverIcon;
		[SerializeField] private Image _questWindowLocationIcon;

#if UNITY_EDITOR
		private void OnValidate()
		{
			CheckComponents();
		}
#endif

		private void CheckComponents()
		{

		}

		private void UpdateQuestText(QuestObject quest)
		{
			_questWindowTitle.text = quest.questTitle;
			_questWindowGiverText.text = quest.questGiver;
			_questWindowLocationText.text = quest.questLocation;
		}
	}
}
