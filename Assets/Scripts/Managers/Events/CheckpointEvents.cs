using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Scenes
{
	public class CheckpointEvents
	{
		public event Action<int> onNewCheckPoint;
		public void NewCheckPoint(int checkpointGUID)
		{
			if (onNewCheckPoint != null)
			{
				onNewCheckPoint.Invoke(checkpointGUID);
			}
		}
	}
}
