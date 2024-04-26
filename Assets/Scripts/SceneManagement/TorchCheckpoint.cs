// using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Saving;
using Arcy.Management;
using UnityEngine;
using UnityEngine.VFX;

namespace Arcy.Scenes
{
	public class TorchCheckpoint : Checkpoint
	{
		[Header("Collider")]
		[SerializeField] private CapsuleCollider _collider;

		[Header("Torch Light VFX")]
		[SerializeField] private VisualEffect _fireVFX;

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_collider == null)
				_collider = TryGetComponent<CapsuleCollider>(out CapsuleCollider hit) ? hit : null;

			if (spawnPoint == null)
				spawnPoint = transform.Find("SpawnPoint").transform;

			if (endPoint == null)
				endPoint = transform.Find("TargetPoint").transform;

			if (_fireVFX == null)
				foreach (Transform child in transform)
				{
					_fireVFX = TryGetComponent<VisualEffect>(out VisualEffect hit) ? hit : null;
					break;
				}

			if (guid == 0)
			{
				guid = Utils.GuidGenerator.guid();
			}
		}
#endif

		private void Start()
		{
			_fireVFX.gameObject.SetActive(true);

			if (GameManager.instance.checkpointManager.mostRecentCheckpointGUID != guid)
			{
				_fireVFX.Stop();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other == Player.PlayerManager.player)
			{
				if (GameManager.instance.checkpointManager.mostRecentCheckpointGUID == guid)
				{
					// This checkpoint is already the current checkpoint
					return;
				}

				GameManager.instance.gameEventManager.checkpointEvents.NewCheckPoint(guid);
			}
		}

		public override void CheckpointUpdated(int checkpointGuid)
		{
			if (checkpointGuid == guid)
			{
				if (!_fireVFX.gameObject.activeInHierarchy)
				{
					_fireVFX.gameObject.SetActive(true);
				}

				_fireVFX.Play();
			}
			else
			{
				_fireVFX.Stop();
			}
		}
	}
}
