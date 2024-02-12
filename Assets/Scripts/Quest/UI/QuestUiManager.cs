using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Arcy.Quest
{
	public class QuestUiManager : MonoBehaviour
	{
		public List<Quest> ongoingQuests = new List<Quest>();
		public List<Quest> finishedQuests = new List<Quest>();

		public Sprite questIcon;
		public TMP_Text questTitleTMP;
		public TMP_Text questGiverTMP;
		public Sprite questGiverIcon;
		public TMP_Text questLocationTMP;
		public Sprite questLocationIcon;

		private void OnValidate()
		{
			foreach (Quest ongoingQuest in QuestManager.instance.ongoingQuests)
			{

			}
		}
	}
}
