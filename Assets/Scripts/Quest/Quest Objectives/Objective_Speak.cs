using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Dialogue;
using UnityEngine;

namespace Arcy.Quests
{
	[CreateAssetMenu(fileName = "Speaking Requirement", menuName = "Quest/Objectives/New Speaking Objectives")]
	public class Objective_Speak : QuestObjectiveBase
	{
		/// <summary>
		/// This is a quest-objective that requires that you speak with an NPC to be finished.
		/// </summary>

		[SerializeField] public string _speakerID;
		[SerializeField] private bool thisObjectiveCanBeSkipped;
		public override bool ThisObjectiveCanBeSkipped { get { return thisObjectiveCanBeSkipped; } set { thisObjectiveCanBeSkipped = value; } }

		public override event Action<QuestObjectiveBase> ObjectiveFinished;

		public override void ObjectiveActivate()
		{
			DialogueManager.DialogueFinished += ConversationFinished;
			Debug.Log(this.name + " is activated");
		}

		private void ConversationFinished(string speakerID)
		{
			if (speakerID == _speakerID)
			{
				Debug.LogWarning("Quest should be updated");
				OnFinish();
			}
		}

		public override void OnFinish()
		{
			DialogueManager.DialogueFinished -= ConversationFinished;

			if (ObjectiveFinished != null)
			{
				ObjectiveFinished(this);
			}
		}
	}
}
