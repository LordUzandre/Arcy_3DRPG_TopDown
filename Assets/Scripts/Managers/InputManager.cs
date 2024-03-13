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
        /// This script acts as a proxy for the PlayerInput component such that the input events the game needs to process will 
        /// be sent through the GameEventManager. This lets any other script in the project easily subscribe to an input action
        /// without having to deal with the PlayerInput component directly.
        /// </summary>

        private GameState _currentGameState;
        private bool _inputEnabled = true;

        private void OnEnable()
        {
            GameEventManager.instance.gameStateManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameEventManager.instance.gameStateManager.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newGameState)
        {
            _currentGameState = newGameState;
        }

        // WASD pressed
        public void OnMove(InputValue inputValue)
        {
            Vector2 vector2 = inputValue.Get<Vector2>();

            switch (_currentGameState)
            {
                case GameState.Freeroam:
                    GameEventManager.instance.inputEvents.WASDPressed(vector2);
                    return;
                case GameState.Pause:
                    GameEventManager.instance.inputEvents.WASDPressed(vector2);
                    return;
            }
        }

        // E-key pressed
        public void OnInteract()
        {
            if (_inputEnabled)
            {
                GameEventManager.instance.inputEvents.InteractKeyPressed();
            }
        }

        // Q-key pressed
        public void OnCancel()
        {
            if (_inputEnabled)
            {
                GameEventManager.instance.inputEvents.CancelKeyPressed();
            }
        }

        // R-shift held
        public void OnRun()
        {
            if (_inputEnabled)
            {
                GameEventManager.instance.inputEvents.OnRunKeyHeld();
            }
        }

        // Esc-key pressed
        public void OnPause()
        {
            if (_inputEnabled)
            {
                GameEventManager.instance.inputEvents.PauseKeyPressed();
            }
        }

        // J-key pressed
        public void OnJournal()
        {
            if (_inputEnabled)
            {
                GameEventManager.instance.inputEvents.JournalKeyPressed();
            }
        }
    }
}