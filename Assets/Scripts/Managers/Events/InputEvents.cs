using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.InputManagement
{
	public class InputEvents
	{
		public event Action<Vector2> WASDInput;
		public void MovePressed(Vector2 moveDir)
		{
			if (WASDInput != null)
			{
				WASDInput(moveDir);
			}
		}

		public event Action InteractionInputPressed;
		public void OnInteractKeyPressed()
		{
			InteractionInputPressed?.Invoke();
		}

		public event Action RunInputHeld;
		public void OnRunKeyHeld()
		{
			RunInputHeld?.Invoke();
		}

		public event Action PauseInputPressed;
		public void OnPauseKeyPressed()
		{
			PauseInputPressed?.Invoke();
		}

		public event Action CancelInputPressed;
		public void OnCancelKeyPressed()
		{
			CancelInputPressed?.Invoke();
		}
	}
}
