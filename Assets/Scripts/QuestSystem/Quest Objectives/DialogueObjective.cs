using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	public class DialogueObjective : QuestObjective
	{
		[Space]
		[SerializeField] private bool _objectiveCanBeSkipped;
		public override bool ObjectiveCanBeSkipped { get { return _objectiveCanBeSkipped; } }

		[Space]
		[SerializeField] private int _dialogueID;

		public override void FinishObjective()
		{
			Management.GameManager.instance.gameEventManager.dialogueEvents.onDialogueFinished -= DialogueFinshed;
		}

		public override void InitializeObjective()
		{
			Debug.Log("");
			Management.GameManager.instance.gameEventManager.dialogueEvents.onDialogueFinished += DialogueFinshed;
		}

		private void DialogueFinshed(int spokenID)
		{
			if (spokenID.Equals(_dialogueID))
			{
				FinishObjective();
			}
		}
	}
}
