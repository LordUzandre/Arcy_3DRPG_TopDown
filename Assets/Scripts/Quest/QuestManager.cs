using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Inventory;

namespace Arcy.Quests
{
	public class QuestManager : ScriptableObject
	{
		public List<QuestObject> availableQuests = new List<QuestObject>();
		public List<QuestObject> ongoingQuests = new List<QuestObject>();
		public List<QuestObject> finishedQuests = new List<QuestObject>();

		public static QuestManager instance;

		[Header("Currently Active Quest (used by map in the future)")]
		[SerializeField] public QuestObject _currentlyActiveQuest;

		// These Actions are used to communicate with the UI
		public static Action<QuestObject> newQuest;
		public static Action<QuestObject> questObjectiveCompleted;
		public static Action<QuestObject> questFinished;
		// public static Action<QuestObject> questFailed; // Necessary?

		private void OnValidate()
		{
			UpdateQuestList();
		}

		// Should be started by the objective or the quest
		public void ObjectiveCompleted(QuestObject quest)
		{
			// Are there more objectives in the quest's "objectives"-list?
			if (quest.questObjectives.Count != 0)
			{
				// Remove current step, activate the next and remember to update UI
			}
			else
			{
				questFinished?.Invoke(quest);
			}

			// Used by Quest UI
			if (questObjectiveCompleted != null)
			{
				questObjectiveCompleted.Invoke(quest);
			}
		}

		public void QuestUpdated()
		{
			// Send a message to UI to update the inGame-UI
			UpdateQuestList();

			foreach (QuestObject quest in ongoingQuests)
			{
				quest.Startup();
			}
		}

		private void UpdateQuestList()
		{
			foreach (QuestObject quest in ongoingQuests)
			{
				quest.isAvailable = false;
				quest.inProgress = true;
				quest.isFinished = false;
			}

			foreach (QuestObject quest in finishedQuests)
			{
				quest.isAvailable = false;
				quest.inProgress = false;
				quest.isFinished = true;
			}
		}
	}
}
