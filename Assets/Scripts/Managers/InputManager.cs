using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Arcy.InputManager
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance { get; private set; }

        public event Action<Vector2> WASDInput;
        public event Action InteractionInputPressed;
        public event Action RunInputHeld;
        public event Action PauseInputPressed;
        public bool inputLocked = false;

        private GameState _currentGameState;
        [SerializeField] private PlayerInput _playerInput;

        private void Awake()
        {
            if (instance == null) { instance = this; } else { Destroy(this); }
        }

        private void OnEnable()
        {
            _playerInput ??= TryGetComponent<PlayerInput>(out PlayerInput plInput) ? plInput : null;

            // subscribe to gameStateManager
            _currentGameState = GameStateManager.Instance.CurrentGameState;
            GameStateManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newGameState)
        {
            _currentGameState = newGameState;

            switch (newGameState)
            {
                case (GameState.Freeroam):
                    _playerInput.SwitchCurrentActionMap("Freeroam");
                    return;
                default:
                    return;
            }
        }

        private void OnDisable()
        {
            GameStateManager.OnGameStateChanged -= OnGameStateChanged;
        }

        private void Update()
        {
            if (!inputLocked)
            {
                // if (_playerInput.currentActionMap == _playerInput.actions.FindActionMap("Freeroam"))
                // {
                OnWASD();
                OnInteractKeyPressed();
                OnRunKeyHeld();
                OnPauseKeyPressed();
                // }
            }
        }

        private void OnWASD()
        {
            WASDInput?.Invoke(_playerInput.actions["move"].ReadValue<Vector2>());
        }

        // When player presses "E"-key
        private void OnInteractKeyPressed()
        {
            if (_playerInput.actions["interact"].WasPressedThisFrame())
            {
                InteractionInputPressed?.Invoke(); // InteractionButton is an Action in PlayerManager.

                if (_playerInput.currentActionMap == _playerInput.actions.FindActionMap("UI"))
                {
                    Debug.Log("aehgaerjhfhafh");
                }
                return;
            }

        }

        private void OnRunKeyHeld()
        {
            if (_playerInput.actions["run"].WasPressedThisFrame())
            {
                RunInputHeld?.Invoke();
            }
        }

        private void OnPauseKeyPressed()
        {

            if (_playerInput.actions["pause"].WasPressedThisFrame())
            {
                PauseInputPressed?.Invoke();
            }
        }

        public void SwapActionMap(InputActionMap actionMap)
        {
            if (actionMap.enabled)
                return;

            //_playerInput.Disable();
            actionMap.Enable();
        }
    }
}