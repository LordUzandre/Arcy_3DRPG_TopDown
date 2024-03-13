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
		/// <summary>
		/// This class acts as a parent class for quest objectives.
		/// Child scripts should be attached to gameobjects and attached as prefabs on the questInfoSO.
		/// The prefabs will be spawned into the scene by QuestManager and destroyed upon completion.
		/// </summary>

		[Header("Status text for Journal UI")]
		[TextArea(2, 6)][SerializeField] public string statusText;

		public abstract bool ThisObjectiveCanBeSkipped { get; set; } // Not yet implemented
		private bool _isFinished = false;
		private string _questId;
		private int _objectiveIndex;

		public abstract void ObjectiveActivate();
		public abstract void OnFinish();

		// Quest Data Load upon startup
		protected abstract void SetQuestObjectiveState(string state);

		// called by Questmanager via Quest-class when this QuestObjective is instatiated (during either Start, Questpoint or QM.AdvanceQuest)
		public void InitializeQuestObjective(string questId, int objectiveIndex, string questObjectiveState)
		{
			this._questId = questId;
			this._objectiveIndex = objectiveIndex;

			if (questObjectiveState != null && questObjectiveState != "")
			{
				SetQuestObjectiveState(questObjectiveState);
			}
		}

		// TODO - so far this is not called
		protected void FinishObjective()
		{
			if (!_isFinished)
			{
				_isFinished = true;
				GameEventManager.instance.questEvents.AdvanceQuest(_questId);

				// Destroy the gameobject after the objective is finished(?)
				Destroy(this.gameObject);
			}
		}

		// Called by QuestPoint when the requirements are met and we press the interact-key
		// Also called by QuestObjective during Start()
		protected void ChangeState(string newState, string newStatus)
		{
			GameEventManager.instance.questEvents.QuestObjectiveStateChange(_questId, _objectiveIndex, new QuestObjectiveState(newState, newStatus));
		}
	}
}
