using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Arcy.InputManagement
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        // Vector2 Wasd
        public event Action<Vector2> WASDInput;

        // Action buttons pressed
        public event Action InteractionInputPressed;
        public event Action CancelInputPressed;
        public event Action RunInputHeld;
        public event Action PauseInputPressed;

        public bool inputLocked = false;
        private bool _freeroamMode;

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
                case (GameState.Pause):
                    _playerInput.SwitchCurrentActionMap("UI");
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
                OnWASD();
                OnInteractKeyPressed();
                OnRunKeyHeld();
                OnPauseKeyPressed();
                OnCancelKeyPressed();
            }
        }

        private void OnWASD()
        {
            switch (_currentGameState)
            {
                case GameState.Freeroam:
                    if (WASDInput != null)
                    {
                        WASDInput.Invoke(_playerInput.actions["move"].ReadValue<Vector2>());
                    }
                    return;
                case GameState.Pause:
                    if (_playerInput.actions["move"].WasPressedThisFrame())
                    {
                        if (WASDInput != null)
                        {
                            WASDInput.Invoke(_playerInput.actions["move"].ReadValue<Vector2>());
                        }
                    }
                    return;
            }
        }

        // When player presses "E"-key
        private void OnInteractKeyPressed()
        {
            if (_playerInput.actions["interact"].WasPressedThisFrame())
            {
                InteractionInputPressed?.Invoke();
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
                if (GameStateManager.Instance.CurrentGameState == GameState.Freeroam)
                {
                    PauseInputPressed?.Invoke();
                    _playerInput.SwitchCurrentActionMap("UI");
                }
            }
        }

        private void OnCancelKeyPressed()
        {
            if (_playerInput.actions["cancel"].WasPressedThisFrame())
            {
                CancelInputPressed?.Invoke();
            }
        }
    }
}