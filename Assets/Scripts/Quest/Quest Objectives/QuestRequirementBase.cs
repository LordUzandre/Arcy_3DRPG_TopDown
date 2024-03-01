using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	public abstract class QuestRequirementBase : ScriptableObject
	{
		[TextArea(2, 6)][SerializeField] public string descriptionText;

		public abstract void OnFinish();
	}
}
