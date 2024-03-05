using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Inventory;

namespace Arcy.Quests
{
	public class QuestManager : ScriptableObject
	{
		public List<Quest> availableQuests = new List<Quest>();
		public List<Quest> ongoingQuests = new List<Quest>();
		public List<Quest> finishedQuests = new List<Quest>();

		public static QuestManager instance;

		[Header("Currently Active Quest (used by map in the future)")]
		[SerializeField] public Quest _currentlyActiveQuest;

		// These Actions are used to communicate with the UI
		public static Action<Quest> newQuest;
		public static Action<Quest> questObjectiveCompleted;
		public static Action<Quest> questFinished;
		// public static Action<QuestObject> questFailed; // Necessary?

		private void OnValidate()
		{
			UpdateQuestList();
		}

		// Should be started by the objective or the quest
		public void ObjectiveCompleted(Quest quest)
		{

		}

		private void UpdateQuestList()
		{
			foreach (Quest quest in ongoingQuests)
			{
				quest.isAvailable = false;
				quest.inProgress = true;
				quest.isFinished = false;
			}

			foreach (Quest quest in finishedQuests)
			{
				quest.isAvailable = false;
				quest.inProgress = false;
				quest.isFinished = true;
			}
		}
	}
}
