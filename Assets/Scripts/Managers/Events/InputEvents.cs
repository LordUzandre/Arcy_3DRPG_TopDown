using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.InputManagement
{
	public class InputEvents
	{
		public event Action<Vector2> onWASDInput;
		public void WASDPressed(Vector2 moveDir)
		{
			if (onWASDInput != null)
			{
				onWASDInput(moveDir);
			}
		}

		public event Action onInteractionInputPressed;
		public void InteractKeyPressed()
		{
			onInteractionInputPressed?.Invoke();
		}

		public event Action onRunInputHeld;
		public void OnRunKeyHeld()
		{
			onRunInputHeld?.Invoke();
		}

		public event Action onPauseInputPressed;
		public void PauseKeyPressed()
		{
			onPauseInputPressed?.Invoke();
		}

		public event Action onCancelInputPressed;
		public void CancelKeyPressed()
		{
			onCancelInputPressed?.Invoke();
		}

		public event Action onJournalInputPressed;
		public void JournalKeyPressed()
		{
			onCancelInputPressed?.Invoke();
		}
	}
}
