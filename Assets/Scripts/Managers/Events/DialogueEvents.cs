using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Dialogue
{
	public class DialogueEvents
	{
		public event Action<string> onDialogueFinished;
		public void DialogueFinished(string speakerID)
		{
			if (onDialogueFinished != null)
			{
				onDialogueFinished(speakerID);
			}
		}

		public event Action onSkipTyping;
		public void SkipTyping()
		{
			if (onSkipTyping != null)
			{
				onSkipTyping.Invoke();
			}
		}
	}
}
