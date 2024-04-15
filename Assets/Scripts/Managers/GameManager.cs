using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Camera;
using Arcy.Dialogue;
using Arcy.InputManagement;
using Arcy.Quests;
using Arcy.Saving;
using Arcy.UI;
using UnityEngine;

namespace Arcy.Management
{
	public class GameManager : MonoBehaviour
	{
		// Singleton
		public static GameManager instance { get; private set; }

		// Non-Monobehaviours
		public GameStateManager gameStateManager { get; private set; }

		[SerializeField] public PersistentDataManager persistentDataManager;
		[SerializeField] public GameEventManager gameEventManager;
		[Space]
		[Header("Initial Game State")]
		[SerializeField] private GameState _startingGameState;

		// TODO - Should run first of all scripts in the scene
		private void Awake()
		{
			if (instance != null)
			{
				Debug.LogError("Found more than one GameManager in the scene.");
				Destroy(gameObject);
				return;
			}

			instance = this;
			DontDestroyOnLoad(gameObject);

			// Initialize all managers
			if (persistentDataManager == null)
			{
				foreach (Transform child in transform)
				{
					if (TryGetComponent<PersistentDataManager>(out PersistentDataManager hit))
					{
						persistentDataManager = hit;
						break;
					}
				}
			}

			if (gameEventManager == null)
			{
				foreach (Transform child in transform)
				{
					if (TryGetComponent<GameEventManager>(out GameEventManager hit))
					{
						gameEventManager = hit;
						break;
					}
				}
			}

			gameStateManager = new GameStateManager();
			gameStateManager.SetState(GameState.RedundantGameState);

		}

		private void Start()
		{
			StartCoroutine(InitialDelay());

			IEnumerator InitialDelay()
			{
				yield return new WaitForSeconds(.5f);
				gameStateManager.SetState(_startingGameState);
			}
		}
	}
}
