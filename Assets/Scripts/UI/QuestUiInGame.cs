using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Arcy.Quests
{
	public class QuestUiInGame : MonoBehaviour
	{
		/// <summary>
		/// This class is attached to the ui-object and listens for any changes from the questmanager.
		/// </summary>

		[Header("Quest Manager")]
		[SerializeField] private QuestManager _questManager;

		[Header("Text-elements")]
		[SerializeField] private TMP_Text _questTMP;
		[SerializeField] private TMP_Text _questUpdatedTMP;
		[SerializeField] private TMP_Text _pressJToLearnMoreTMP;

		[Header("Quest objectives(?)")]
		[SerializeField] private QuestObjective _previousQuestStep;
		[SerializeField] private QuestObjective _currentQuestStep;

		[Header("Parent GameObject")]
		[SerializeField] private GameObject _parentObject;
	}
}
