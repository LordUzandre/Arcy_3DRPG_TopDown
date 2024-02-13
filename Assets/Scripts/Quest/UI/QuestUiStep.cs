using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Quest
{
	public class QuestUiStep : MonoBehaviour
	{
		[SerializeField] private TMP_Text _descriptiveText;
		[SerializeField] private QuestRequirementBase _questStepRequirement;

		private void Start()
		{
			_descriptiveText ??= GetComponentInChildren<TMP_Text>();

			_descriptiveText.text = _questStepRequirement.requirementDescription;
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			_descriptiveText ??= GetComponentInChildren<TMP_Text>();
		}
#endif
	}
}
