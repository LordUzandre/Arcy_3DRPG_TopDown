using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Management;
using UnityEngine;

namespace Arcy.Quests
{
	public class QuestPoint : MonoBehaviour
	{
		/// <summary>
		/// TODO: This needs to be replaced with the interacto system instead.
		/// </summary>

		[Header("Config")]
		[SerializeField] private bool _startPoint;
		[SerializeField] private bool _finishPoint;

		[Header("Quest")]
		[SerializeField] private QuestSO _questInfoForPoint;
		private bool _playerIsNear = false;
		private int _questId;
		private QuestObjectiveEnum _currentQuestState;

		private void Awake()
		{
			_questId = _questInfoForPoint.questGuid;
		}

		private void OnEnable()
		{
			// GameManager.instance.gameEventManager.questEvents.onQuestStateChange += QuestStateChange;
			GameManager.instance.gameEventManager.inputEvents.onInteractionInputPressed += SubmitPressed;
		}

		private void OnDisable()
		{
			// GameManager.instance.gameEventManager.questEvents.onQuestStateChange -= QuestStateChange;
			GameManager.instance.gameEventManager.inputEvents.onInteractionInputPressed -= SubmitPressed;
		}

		private void QuestStateChange(Quest quest)
		{
			// Only update the quest state if this point has the corresponding quest
			if (quest.QuestObject.questGuid.Equals(_questId))
			{
				_currentQuestState = quest.CurrentStatusEnum;
				Debug.Log("Quest ith id: " + _questId + " updated to state: " + _currentQuestState);
			}
		}

		// TODO - implement into the interaction-system
		private void SubmitPressed()
		{
			if (!_playerIsNear)
				return;

			// Start or finish a quest
			if (_currentQuestState.Equals(QuestObjectiveEnum.CAN_START) && _startPoint)
			{
				GameManager.instance.gameEventManager.questEvents.StartQuest(_questId);
			}
			else if (_currentQuestState.Equals(QuestObjectiveEnum.CAN_FINISH) && _finishPoint)
			{
				GameManager.instance.gameEventManager.questEvents.FinishQuest(_questId);
			}

		}
	}
}
