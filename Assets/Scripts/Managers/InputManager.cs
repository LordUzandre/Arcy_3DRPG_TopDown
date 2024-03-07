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

        private void OnEnable()
        {
            // subscribe to gameStateManager
            _currentGameState = GameStateManager.Instance.CurrentGameState;
            GameStateManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameStateManager.OnGameStateChanged -= OnGameStateChanged;
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

        public void OnWASD(InputAction.CallbackContext context)
        {
            switch (_currentGameState)
            {
                case GameState.Freeroam:
                    if (context.performed || context.canceled)
                    {
                        GameEventManager.instance.inputEvents.WASDPressed(context.ReadValue<Vector2>());
                    }
                    return;
                case GameState.Pause:
                    if (context.started)
                    {
                        GameEventManager.instance.inputEvents.WASDPressed(context.ReadValue<Vector2>());
                    }
                    return;
            }
        }

        public void OnInteractKeyPressed(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameEventManager.instance.inputEvents.InteractKeyPressed();
            }
        }

        public void OnCancelKeyPressed(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameEventManager.instance.inputEvents.CancelKeyPressed();
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
            if (context.started)
            {
                GameEventManager.instance.inputEvents.PauseKeyPressed();
            }
        }

        public void OnJournalKeyPressed(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameEventManager.instance.inputEvents.JournalKeyPressed();
            }
        }
    }
}