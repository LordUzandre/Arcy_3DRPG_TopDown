using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Arcy.Management;

namespace Arcy.InputManagement
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        /// <summary>
        /// This script acts as a proxy for the PlayerInput component
        /// such that the input events the game needs to proces will 
        /// be sent through the GameEventManager. This lets any other
        /// script in the project easily subscribe to an input action
        /// without having to deal with the PlayerInput component directly.
        /// </summary>

        // public bool inputLocked = false;
        // private bool _freeroamMode;

        private GameState _currentGameState;
        // [SerializeField] private PlayerInput _playerInput;

        private void OnEnable()
        {
            // _playerInput ??= TryGetComponent<PlayerInput>(out PlayerInput plInput) ? plInput : null;

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
                    return;
                case (GameState.Pause):
                    return;
                default:
                    return;
            }
        }

        private void OnDisable()
        {
            GameStateManager.OnGameStateChanged -= OnGameStateChanged;
        }

        public void OnWASD(InputAction.CallbackContext context)
        {
            switch (_currentGameState)
            {
                case GameState.Freeroam:
                    if (context.performed || context.canceled)
                    {
                        GameEventManager.instance.inputEvents.MovePressed(context.ReadValue<Vector2>());
                    }
                    return;
                case GameState.Pause:
                    if (context.started)
                    {
                        GameEventManager.instance.inputEvents.MovePressed(context.ReadValue<Vector2>());
                    }
                    return;
            }
        }

        public void OnInteractKeyPressed(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameEventManager.instance.inputEvents.OnInteractKeyPressed();
            }
        }

        public void OnCancelKeyPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameEventManager.instance.inputEvents.OnCancelKeyPressed();
            }
        }

        public void OnRunKeyHeld(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameEventManager.instance.inputEvents.OnRunKeyHeld();
            }
        }

        public void OnPauseKeyPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameEventManager.instance.inputEvents.OnPauseKeyPressed();
            }
        }
    }
}