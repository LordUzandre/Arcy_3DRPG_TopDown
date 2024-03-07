using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Quests
{
	public class QuestUiStep : MonoBehaviour
	{
		[SerializeField] private TMP_Text _descriptiveText;
		[SerializeField] private QuestObjective _questStepRequirement;

		private void Start()
		{
			if (_descriptiveText == null)
			{
				GetComponentInChildren<TMP_Text>();
			}

			_descriptiveText.text = _questStepRequirement.statusText;
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			_descriptiveText ??= GetComponentInChildren<TMP_Text>();
		}
#endif
	}
}
