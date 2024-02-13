using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quest
{
	public class QuestRequirementBase : ScriptableObject
	{
		[SerializeField] public UnityEngine.UI.Image spriteIcon;
		[SerializeField] public string requirementDescription;
	}
}
