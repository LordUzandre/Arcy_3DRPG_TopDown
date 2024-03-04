using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	public abstract class QuestObjectiveBase : ScriptableObject
	{
		[TextArea(2, 6)][SerializeField] public string descriptionText;

		public abstract bool ThisObjectiveCanBeSkipped { get; set; }
		public abstract void ObjectiveActivate();
		public abstract void OnFinish();

		public abstract event Action<QuestObjectiveBase> ObjectiveFinished;
	}
}
