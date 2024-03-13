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
		public static GameManager instance { get; private set; }

		public PersistentDataManager _persistentDataManager { get; private set; }
		public GameEventManager _gameEventManager { get; private set; }
		public GameStateManager _gameStateManager { get; private set; }

		// [SerializeField] private DialogueManager _dialogueManager;
		// [SerializeField] private InputManager _inputManager;
		// [SerializeField] private CameraManager _cameraManager;
		// [SerializeField] private PauseMenuManager _pauseMenuManager;
		// [SerializeField] private QuestManager _questManager;

		[Header("Initial Game State")]
		[SerializeField] private GameState _startingGameState;

		// TODO - Should run first of all scripts in the scene
		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else
			{
				Debug.LogError("Found more than one GameManager in the scene.");
				Destroy(gameObject);
			}

			// Initialize all managers
			// _gameEventManager = new GameEventManager();
			// _gameStateManager = new GameStateManager();

			GameEventManager.instance.gameStateManager.SetState(GameState.RedundantGameState);
		}

		private void Start()
		{
			StartCoroutine(InitialDelay());

			IEnumerator InitialDelay()
			{
				yield return new WaitForSeconds(.5f);
				GameEventManager.instance.gameStateManager.SetState(_startingGameState);
			}
		}
	}
}
