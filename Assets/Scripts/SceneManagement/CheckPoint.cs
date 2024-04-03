using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace Arcy.SceneManagement
{
	public class CheckPoint : MonoBehaviour
	{
		[Header("Collider")]
		[SerializeField] private CapsuleCollider _collider;

		[Header("Spawn Points")]
		[SerializeField] public Transform spawnPoint;
		[SerializeField] public Transform endPoint;

		[Header("Torch Light VFX")]
		[SerializeField] private UnityEngine.VFX.VisualEffect _fireVFX;

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
