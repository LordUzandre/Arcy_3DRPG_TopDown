using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	[System.Serializable]
	public class QuestData
	{
		/// <summary>
		/// This is the data that gets saved/loaded by QuestManager in the "original" saving system.
		/// </summary>

		public QuestStateEnum state;
		public int questObjectiveIndex;
		public QuestObjectiveState[] questObjectiveStates;

		// Constructor
		public QuestData(QuestStateEnum state, int questObjectiveIndex, QuestObjectiveState[] questObjectiveStates)
		{
			this.state = state;
			this.questObjectiveIndex = questObjectiveIndex;
			this.questObjectiveStates = questObjectiveStates;
		}
	}
}
