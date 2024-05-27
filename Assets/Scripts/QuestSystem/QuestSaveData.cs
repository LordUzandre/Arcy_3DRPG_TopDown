using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	[System.Serializable]
	public class QuestSaveData
	{
		/// <summary>
		/// This is the data that gets saved/loaded by QuestManager in the "original" saving system.
		/// </summary>

		public QuestObjectiveEnum State; // enum
		public int ObjectiveIndex; // int

		// Constructor
		public QuestSaveData(QuestObjectiveEnum state, int questObjectiveIndex)
		{
			State = state;
			ObjectiveIndex = questObjectiveIndex;
		}
	}


}
