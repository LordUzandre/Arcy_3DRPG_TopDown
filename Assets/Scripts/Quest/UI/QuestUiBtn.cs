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
		public QuestObject _quest;
		[Space]
		public TMP_Text _questTitleTMP;
		public TMP_Text _questGiverTMP;
		public Sprite _questIcon;

		private void SpawnNewBtn(QuestObject quest)
		{
			_quest = quest;
			_questTitleTMP.text = quest.questTitle;
			_questGiverTMP.text = quest.questGiver;
			_questIcon = quest.questIcon;
		}
	}
}
