using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quest
{
	[CreateAssetMenu(fileName = "QuestManager", menuName = "Quest/New QuestManager")]
	public class QuestManager : ScriptableObject
	{
		public List<Quest> ongoingQuests = new List<Quest>();
		public List<Quest> finishedQuests = new List<Quest>();

		public static QuestManager instance;

		private void Awake()
		{
			if (instance == null) { instance = this; } else { Destroy(this); }
		}
	}
}
