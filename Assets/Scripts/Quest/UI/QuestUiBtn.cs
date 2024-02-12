using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Quest;
using TMPro;

namespace Arcy.UI
{
	public class QuestUiBtn : MenuBtn
	{
		public QuestObject quest;

		public Sprite questIcon;
		public TMP_Text questTitleTMP;
		public TMP_Text questGiverTMP;
		public Sprite questGiverIcon;
		public TMP_Text questLocationTMP;
		public Sprite questLocationIcon;

		private void SpawnNewBtn(QuestObject quest)
		{
			questTitleTMP.text = quest.questTitle;
			questGiverTMP.text = quest.questGiver;
			questLocationTMP.text = quest.questLocation;
		}
	}
}
