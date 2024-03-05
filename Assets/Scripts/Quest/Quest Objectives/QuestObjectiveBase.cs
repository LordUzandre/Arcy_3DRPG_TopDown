using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Arcy.Quests
{
	public abstract class QuestObjectiveBase : ScriptableObject
	{
		[TextArea(2, 6)][SerializeField] public string descriptionText;

		public abstract bool ThisObjectiveCanBeSkipped { get; set; }
		private bool isFinished = false;
		public abstract void ObjectiveActivate();
		public abstract void OnFinish();

		public abstract event Action<QuestObjectiveBase> ObjectiveFinished;

		protected void FinishObjective()
		{
			if (!isFinished)
			{
				isFinished = true;

				// Advance the quest forward now that the step is finished

				// Destroy(this.gameObject);
			}
		}
	}
}
