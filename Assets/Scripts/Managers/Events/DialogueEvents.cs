using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Dialogue
{
	public class DialogueEvents
	{
		public event Action<string> onDialogueFinish;
		public void DialogueFinished(string id)
		{
			if (onDialogueFinish != null)
			{
				onDialogueFinish(id);
			}
		}
	}
}
