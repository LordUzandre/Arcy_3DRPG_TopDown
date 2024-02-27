using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	public abstract class QuestRequirementBase : ScriptableObject
	{
		//private Quest _quest;
		[SerializeField] public UnityEngine.UI.Image spriteIcon;
		[Space]
		[TextArea(2, 6)][SerializeField] public string descriptionText;

		public abstract void OnFinish();
	}
}
