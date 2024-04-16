using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	public class QuestEvents
	{
		public event Action<int> onStartQuest;
		public void StartQuest(int questId)
		{
			if (onStartQuest != null)
			{
				onStartQuest(questId);
			}
		}

		public event Action<int> onAdvanceQuest;
		public void AdvanceQuest(int questId)
		{
			if (onAdvanceQuest != null)
			{
				onAdvanceQuest(questId);
			}
		}

		public event Action<int> onFinishQuest;
		public void FinishQuest(int questId)
		{
			if (onFinishQuest != null)
			{
				onFinishQuest(questId);
			}
		}

		public event Action<Quest> onQuestStateChange;
		public void QuestStateChange(Quest quest)
		{
			if (onQuestStateChange != null)
			{
				onQuestStateChange(quest);
			}
		}

		public event Action<int, int, QuestObjectiveState> onQuestObjectiveStateChange;
		public void QuestObjectiveStateChange(int questId, int stepIndex, QuestObjectiveState questStepState)
		{
			if (onQuestObjectiveStateChange != null)
			{
				onQuestObjectiveStateChange(questId, stepIndex, questStepState);
			}
		}
	}
}
