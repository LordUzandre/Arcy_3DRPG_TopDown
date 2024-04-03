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

		public abstract bool ThisObjectiveCanBeSkipped { get; set; } // TODO: Not yet implemented
		private bool _isFinished = false;
		private string _questId;
		private int _objectiveIndex;

		public virtual void Initialize(string questId, int objectiveIndex, string questObjectiveState)
		{
			_questId = questId;
			_objectiveIndex = objectiveIndex;
		}

		public virtual void FinishQuest()
		{

		}

		protected void ChangeState(string newState, string newUiStatus)
		{
			GameEventManager.instance.questEvents.QuestObjectiveStateChange(_questId, _objectiveIndex, new QuestObjectiveState(newState, newUiStatus));
		}

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
	}
}
