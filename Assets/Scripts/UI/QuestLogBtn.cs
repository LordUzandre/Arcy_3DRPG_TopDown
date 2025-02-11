using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Quests;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

namespace Arcy.UI
{
	public class QuestLogBtn : MonoBehaviour, ISelectHandler
	{
		public Button button { get; private set; }
		private TMP_Text _btnTMP;

		// Because the btn is being instantiated it may be disabled during startup
		// Instead it's being manually initialized here
		public void Initialize(string displayName, UnityAction selectAction)
		{
			button = GetComponent<Button>();
			_btnTMP = GetComponentInChildren<TMP_Text>();

			_btnTMP.text = displayName;
			_onSelectAction = selectAction;
		}

		private UnityAction _onSelectAction;
		public void OnSelect(BaseEventData eventData)
		{
			_onSelectAction();
		}

		public void SetState(QuestObjectiveEnum state)
		{
			switch (state)
			{
				case QuestObjectiveEnum.REQUIREMENTS_NOT_MET:
				case QuestObjectiveEnum.CAN_START:
					_btnTMP.color = Color.red;
					break;
				case QuestObjectiveEnum.STARTED:
				case QuestObjectiveEnum.CAN_FINISH:
					_btnTMP.color = Color.yellow;
					break;
				case QuestObjectiveEnum.FINISHED:
					_btnTMP.color = Color.green;
					break;
				default:
					Debug.LogWarning("Quest State not recognized by switch statement");
					break;
			}
		}
	}
}
