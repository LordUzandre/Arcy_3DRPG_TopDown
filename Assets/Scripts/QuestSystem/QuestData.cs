using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	[System.Serializable]
	public class QuestData
	{
		public QuestStateEnum state;
		public int questObjectiveIndex;
		public QuestObjectiveState[] questObjectiveStates;

		public QuestData(QuestStateEnum state, int questObjectiveIndex, QuestObjectiveState[] questObjectiveStates)
		{
			this.state = state;
			this.questObjectiveIndex = questObjectiveIndex;
			this.questObjectiveStates = questObjectiveStates;
		}
	}
}
