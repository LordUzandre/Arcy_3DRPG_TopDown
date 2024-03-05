using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Quests;
using TMPro;
using Unity.VisualScripting;

namespace Arcy.UI
{
	public class QuestUiBtn : MenuBtn
	{
		[Header("Quest")]
		[SerializeField] private Quest _quest;
		[Space]
		[SerializeField] private TMP_Text _questTitleTMP;
		[SerializeField] private TMP_Text _questGiverTMP;
		[SerializeField] private UnityEngine.UI.Image _questIcon;

		// Method is called by Quest UI Manager when spawning a new btn
		public void NewBtnSpawned(Quest quest)
		{
			_quest = quest;
			_questTitleTMP.text = quest.questDisplayNAme;
			_questGiverTMP.text = quest.questGiver;
			_questIcon = quest.questIcon;
		}
	}
}
