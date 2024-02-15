using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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
		[Header("Right Window Panel")]
		[SerializeField] private QuestWindow _questWindow;
		[SerializeField] private Transform onGoingListParent;
	}
}
