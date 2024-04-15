using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Saving
{
	public class CheckpointEvents
	{
		public event Action<string> onNewCheckPoint;
		public void NewCheckPoint(string checkpointGUID)
		{
			if (onNewCheckPoint != null)
			{
				onNewCheckPoint.Invoke(checkpointGUID);
			}
		}
	}
}
