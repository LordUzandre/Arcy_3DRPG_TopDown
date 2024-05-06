using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Arcy.Saving;
using Arcy.Management;

namespace Arcy.Scenes
{
	public class CheckpointManager : MonoBehaviour, ISaveableEntity
	{
		[Header("Config")]
		[SerializeField] private bool _useSaveData;
		[Header("Most recent checkpoints")]
		[SerializeField] public int mostRecentCheckpointGUID;
		[Header("Scene-related")]
		[SerializeField] SceneSO sceneData;
		[SerializeField] public Checkpoint[] allCheckpointsInScene;

		// MARK: PUBLIC:
		// MARK: PRIVATE:
		private void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
			GameManager.instance.gameEventManager.checkpointEvents.onNewCheckPoint += NewCheckpoint;
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
			GameManager.instance.gameEventManager.checkpointEvents.onNewCheckPoint -= NewCheckpoint;
		}

		private void NewCheckpoint(int checkpointGUID)
		{
			mostRecentCheckpointGUID = checkpointGUID;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			allCheckpointsInScene = FindAllCheckpoints();
		}

		private Checkpoint[] FindAllCheckpoints()
		{
			List<Checkpoint> myList = new List<Checkpoint>();
			Transform root = GameObject.Find("Checkpoints").transform;

			if (root == null)
			{
				return null;
			}

			foreach (Transform child in root)
			{
				if (child.TryGetComponent<Checkpoint>(out Checkpoint checkpoint))
				{
					myList.Add(checkpoint);
				}
			}

			return allCheckpointsInScene = myList.ToArray();
		}

#if UNITY_EDITOR
		private void OnValidate()
		{

		}

		// MARK: Save/Load
		public void LoadData(SaveData loadData)
		{
			mostRecentCheckpointGUID = loadData.mostRecentCheckpoint;
		}

		public void SaveData(SaveData saveData)
		{
			saveData.mostRecentCheckpoint = mostRecentCheckpointGUID;
		}
#endif
	}
}