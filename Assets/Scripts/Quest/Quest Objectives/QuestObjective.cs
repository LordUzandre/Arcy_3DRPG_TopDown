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
		public abstract void ObjectiveActivate();
		public abstract void OnFinish();

		public abstract event Action<QuestObjective> ObjectiveFinished;

		public void InitializeQuestObjective(string questId)
		{
			this._questId = questId;
		}

		protected void FinishObjective()
		{
			if (!_isFinished)
			{
				_isFinished = true;
				GameEventManager.instance.questEvents.AdvanceQuest(_questId);

				// Advance the quest forward now that the step is finished

				// Destroy(this.gameObject);
			}
		}
	}
}
