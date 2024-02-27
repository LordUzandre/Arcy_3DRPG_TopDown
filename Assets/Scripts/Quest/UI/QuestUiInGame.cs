using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Arcy.Quests
{
	public class QuestUiInGame : MonoBehaviour
	{
		[SerializeField] private TMP_Text _questTMP;
		[SerializeField] private TMP_Text _updatedTMP;
		[SerializeField] private TMP_Text _pressJToLearnMoreTMP;

		[SerializeField] private QuestRequirementBase _previousQuestStep;
		[SerializeField] private QuestRequirementBase _currentQuestStep;

		[SerializeField] private GameObject _questUiStepPrefab;
		[SerializeField] private GameObject _questStepsVertLayout;
	}
}
