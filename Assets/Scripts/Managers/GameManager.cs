using System.Collections;
using System.Collections.Generic;
using Arcy.Camera;
using Arcy.Dialogue;
using Arcy.InputManagement;
using Arcy.Inventory;
using Arcy.Quests;
using Arcy.Scenes;
using Arcy.Saving;
using UnityEngine;

namespace Arcy.Management
{
	public class GameManager : MonoBehaviour
	{
		// Singleton
		public static GameManager instance { get; private set; }

		// Non-Monobehaviours
		public GameStateManager gameStateManager { get; private set; }

		[Header("Managers")]
		[SerializeField] public CameraManager cameraManager;
		[SerializeField] public CheckpointManager checkpointManager;
		[SerializeField] public DialogueManager dialogueManager;
		[SerializeField] public GameEventManager gameEventManager;
		[SerializeField] public InputManager inputManager;
		[SerializeField] public InventoryManager inventoryManager;
		[SerializeField] public SaveDataManager persistentDataManager;
		[SerializeField] public QuestManager questManager;
		[Space]
		[Header("Initial Game State")]
		[SerializeField] private GameState _startingGameState;

#if UNITY_EDITOR
		private void OnValidate()
		{
			CheckComponents();
		}
#endif

		private void CheckComponents()
		{
			// Initialize all managers
			cameraManager ??= GetComponentInChildren<CameraManager>();
			checkpointManager ??= GetComponentInChildren<CheckpointManager>();
			dialogueManager ??= GetComponentInChildren<DialogueManager>();
			gameEventManager ??= GetComponentInChildren<GameEventManager>();
			inputManager ??= GetComponentInChildren<InputManager>();
			inventoryManager ??= GetComponentInChildren<InventoryManager>();
			persistentDataManager ??= GetComponentInChildren<SaveDataManager>();
			questManager ??= GetComponentInChildren<QuestManager>();
		}

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

			CheckComponents();
			gameEventManager.Initialize();

			gameStateManager = new GameStateManager();
			gameStateManager.SetState(GameState.RedundantGameState);

		}

		private void Start()
		{
			StartCoroutine(InitialDelay());

			IEnumerator InitialDelay()
			{
				yield return new WaitForSeconds(2f);
				gameStateManager.SetState(_startingGameState);
			}
		}
	}
}
