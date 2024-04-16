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

		[Header("Quest")]
		[SerializeField] private QuestInfoSO questInfoForPoint;
		private bool _playerIsNear = false;
		private int questId;
		private QuestStateEnum currentQuestState;

		[SerializeField] private bool startPoint;
		[SerializeField] private bool finishPoint;

		private void Awake()
		{
			questId = questInfoForPoint.guid;
		}

		private void OnEnable()
		{
			GameManager.instance.gameEventManager.questEvents.onQuestStateChange += QuestStateChange;
			GameManager.instance.gameEventManager.inputEvents.onInteractionInputPressed += SubmitPressed;
		}

		private void OnDisable()
		{
			GameManager.instance.gameEventManager.questEvents.onQuestStateChange -= QuestStateChange;
			GameManager.instance.gameEventManager.inputEvents.onInteractionInputPressed -= SubmitPressed;
		}

		private void QuestStateChange(Quest quest)
		{
			// Only update the quest state if this point has the corresponding quest
			if (quest.questSO.guid.Equals(questId))
			{
				currentQuestState = quest.currentStatusEnum;
				Debug.Log("Quest ith id: " + questId + " updated to state: " + currentQuestState);
			}
		}

		// TODO - implement into the interaction-system
		private void SubmitPressed()
		{
			if (!_playerIsNear)
				return;

			if (currentQuestState.Equals(QuestStateEnum.CAN_START) && startPoint)
			{
				GameManager.instance.gameEventManager.questEvents.StartQuest(questId);
			}
			else if (currentQuestState.Equals(QuestStateEnum.CAN_FINISH) && finishPoint)
			{
				GameManager.instance.gameEventManager.questEvents.FinishQuest(questId);
			}

		}
	}
}
