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

		public void SetState(QuestState state)
		{
			switch (state)
			{
				case QuestState.REQUIREMENTS_NOT_MET:
				case QuestState.CAN_START:
					_btnTMP.color = Color.red;
					break;
				case QuestState.IN_PROGRESS:
				case QuestState.CAN_FINISH:
					_btnTMP.color = Color.yellow;
					break;
				case QuestState.FINISHED:
					_btnTMP.color = Color.green;
					break;
				default:
					Debug.LogWarning("Quest State not recognized by switch statement");
					break;
			}
		}
	}
}
