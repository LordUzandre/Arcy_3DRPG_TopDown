using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	[System.Serializable]
	public class QuestObjectiveState
	{
		public string State;
		public string Status;

		public QuestObjectiveState(string state, string status)
		{
			State = state;
			Status = status;
		}

		public QuestObjectiveState()
		{
			State = "";
			Status = "";
		}
	}
}
