using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Management;
using Unity.VisualScripting;
using UnityEngine;

namespace Arcy.Quests
{
	public abstract class QuestObjective : MonoBehaviour
	{
		[TextArea(2, 6)][SerializeField] public string descriptionText;

		public abstract bool ThisObjectiveCanBeSkipped { get; set; }
		private bool _isFinished = false;
		private string _questId;
		private int _objectiveIndex;

		public abstract void ObjectiveActivate();
		public abstract void OnFinish();

		public void InitializeQuestObjective(string questId, int objectiveIndex, string questObjectiveState)
		{
			this._questId = questId;
			this._objectiveIndex = objectiveIndex;

			if (questObjectiveState != null && questObjectiveState != "")
			{
				SetQuestObjectiveState(questObjectiveState);
			}
		}

		protected void FinishObjective()
		{
			if (!_isFinished)
			{
				_isFinished = true;
				GameEventManager.instance.questEvents.AdvanceQuest(_questId);
				// Destroy(this.gameObject);
			}
		}

		protected void ChangeState(string newState)
		{
			GameEventManager.instance.questEvents.QuestObjectiveStateChange(_questId, _objectiveIndex, new QuestObjectiveState(newState));
		}

		// Quest Data Load upon startup
		protected abstract void SetQuestObjectiveState(string state);
	}
}
