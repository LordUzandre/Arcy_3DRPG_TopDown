using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Arcy.Quest;
using TMPro;

namespace Arcy.UI
{
	public class QuestWindow : MonoBehaviour
	{
		[SerializeField] private TMP_Text _titleTMP;
		[SerializeField] private TMP_Text _questGiverTMP;
		[SerializeField] private TMP_Text _questLocationTMP;
		[SerializeField] private TMP_Text _questDescriptionTMP;
		[Header("Requirement")]
		[SerializeField] private List<GameObject> _questRequirements = new List<GameObject>();
		[Header("Rewards")]
		[SerializeField] private List<GameObject> _questRewards = new List<GameObject>();

		private void UpdateQuestText(QuestObject quest)
		{
			_titleTMP.text = quest.questTitle;
			_questGiverTMP.text = quest.questGiver;
			_questLocationTMP.text = quest.questLocation;
		}

	}
}
