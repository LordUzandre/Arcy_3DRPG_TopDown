using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Player
{
	public class PlayerEvents
	{
		public event Action onPlayerResumeControl;
		public void PlayerResumeControl()
		{
			if (onPlayerResumeControl != null)
			{
				onPlayerResumeControl.Invoke();
			}
		}

		public event Action onPlayerLevelUp;
		public void PlayerLevelUp()
		{
			if (onPlayerLevelUp != null)
			{
				onPlayerLevelUp.Invoke();
			}
		}

		public event Action<Vector3> onPlayerMoveToPosition;
		public void PlayerMoveToPosition(Vector3 newPos)
		{
			if (onPlayerMoveToPosition != null)
			{
				onPlayerMoveToPosition.Invoke(newPos);
			}
		}
	}
}
