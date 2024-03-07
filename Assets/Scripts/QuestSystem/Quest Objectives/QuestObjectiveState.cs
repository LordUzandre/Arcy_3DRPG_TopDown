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
		public string status;

		public QuestObjectiveState(string state, string status)
		{
			this.state = state;
			this.status = status;
		}

		public QuestObjectiveState()
		{
			this.state = "";
			this.status = "";
		}
	}
}
