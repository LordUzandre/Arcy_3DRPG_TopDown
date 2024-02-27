using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	public class QuestManager : ScriptableObject
	{
		public List<QuestObject> ongoingQuests = new List<QuestObject>();
		public List<QuestObject> finishedQuests = new List<QuestObject>();

		public static QuestManager instance;

		[Header("Currently Active Quest")]
		[SerializeField] public QuestObject _currentlyActiveQuest; // Mainly used for map and waypoint reference in the future.

		public static Action<QuestObject> questObjectiveCompleted;
		public static Action<QuestObject> questFinished;
		// public static Action<QuestObject> questFailed; // Needed?

		// Should be started by the quest-requirement or the quest
		private void objectiveCompleted(QuestObject quest)
		{
			if (questObjectiveCompleted != null)
			{
				questObjectiveCompleted.Invoke(quest);
			}

			// Are there more objectives in the quest's "objectives"-list?
			if (quest.questObjectives.Count != 0)
			{
				// Remove current step, activate the next and remember to update UI
			}
			else
			{
				questFinished?.Invoke(quest);
			}
		}
	}
}
