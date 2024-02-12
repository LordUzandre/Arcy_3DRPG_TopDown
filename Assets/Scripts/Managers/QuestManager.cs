using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quest
{
	public class QuestManager : ScriptableObject
	{
		public List<QuestObject> ongoingQuests = new List<QuestObject>();
		public List<QuestObject> finishedQuests = new List<QuestObject>();

		public static QuestManager instance;
	}
}
