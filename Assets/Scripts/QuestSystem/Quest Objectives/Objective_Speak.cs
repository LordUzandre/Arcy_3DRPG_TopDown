using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Dialogue;
using Arcy.Management;
using UnityEngine;

namespace Arcy.Quests
{
	public class Objective_Speak : QuestObjective
	{
		/// <summary>
		/// This is a quest-objective that requires that you speak with an NPC to be finished.
		/// </summary>

		[SerializeField] public string _speakerID;
		[SerializeField] private bool thisObjectiveCanBeSkipped;
		public override bool ThisObjectiveCanBeSkipped { get { return thisObjectiveCanBeSkipped; } set { thisObjectiveCanBeSkipped = value; } }

		public override void ObjectiveActivate()
		{
			GameEventManager.instance.dialogueEvents.onDialogueFinish += ConversationFinished;
		}

		public override void OnFinish()
		{
			GameEventManager.instance.dialogueEvents.onDialogueFinish += ConversationFinished;
		}

		private void ConversationFinished(string speakerID)
		{
			if (speakerID == _speakerID)
			{
				Debug.LogWarning("Quest should be updated");
				OnFinish();
			}
		}

		private void UpdateState()
		{
			// string state = coindCollected.ToString();
			// ChangeState(state);
		}

		protected override void SetQuestObjectiveState(string state)
		{
			// this.coinsCollected = System.Int32.Parse(state);
			UpdateState();
		}
	}
}
