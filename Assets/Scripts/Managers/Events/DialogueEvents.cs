using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Dialogue
{
	public class DialogueEvents
	{
		public event Action<int> onDialogueFinished;
		public void DialogueFinished(int speakerID)
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
