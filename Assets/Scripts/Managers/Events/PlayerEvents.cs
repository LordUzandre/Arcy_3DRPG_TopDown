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
	}
}
