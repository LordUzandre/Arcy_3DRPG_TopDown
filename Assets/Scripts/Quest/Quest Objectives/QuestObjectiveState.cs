using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	[System.Serializable]
	public class QuestObjectiveState
	{
		public string state;

		public QuestObjectiveState(string state)
		{
			this.state = state;
		}

		public QuestObjectiveState()
		{
			this.state = "";
		}
	}
}
