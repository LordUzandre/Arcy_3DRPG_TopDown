using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Management;
using System;

namespace Arcy.Scenes
{
	public abstract class Checkpoint : MonoBehaviour
	{
		[Header("CheckPoint GUID")]
		[SerializeField] public int guid;

		[Header("Spawn Points")]
		[SerializeField] public Transform spawnPoint;
		[SerializeField] public Transform endPoint;

		private void OnEnable()
		{
			GameManager.instance.gameEventManager.checkpointEvents.onNewCheckPoint += CheckpointUpdated;
		}

		private void OnDisable()
		{
			GameManager.instance.gameEventManager.checkpointEvents.onNewCheckPoint -= CheckpointUpdated;
		}

		public abstract void CheckpointUpdated(int guid);
	}
}
