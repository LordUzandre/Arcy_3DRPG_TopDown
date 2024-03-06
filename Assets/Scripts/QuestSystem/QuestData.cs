using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	[System.Serializable]
	public class QuestData
	{
		public QuestState state;
		public int questObjectiveIndex;
		public QuestObjectiveState[] questObjectiveStates;

		public QuestData(QuestState state, int questObjectiveIndex, QuestObjectiveState[] questObjectiveStates)
		{
			this.state = state;
			this.questObjectiveIndex = questObjectiveIndex;
			this.questObjectiveStates = questObjectiveStates;
		}
	}
}
