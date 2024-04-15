//using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Saving;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace Arcy.SceneManagement
{
	public class CheckPoint : MonoBehaviour
	{
		[Header("CheckPoint GUID")]
		[SerializeField] private string _checkpointGUID;

		[Header("Collider")]
		[SerializeField] private CapsuleCollider _collider;

		[Header("Spawn Points")]
		[SerializeField] public Transform spawnPoint;
		[SerializeField] public Transform endPoint;

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

			if (_checkpointGUID == null)
			{
				_checkpointGUID = System.Guid.NewGuid().ToString();
			}
		}
#endif

		private void Start()
		{
			_fireVFX.Stop();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				InitializeCheckpoint();
			}
		}

		private void InitializeCheckpoint()
		{
			if (!_fireVFX.gameObject.activeInHierarchy)
			{
				_fireVFX.gameObject.SetActive(true);
			}

			_fireVFX.Play();
		}
	}
}
