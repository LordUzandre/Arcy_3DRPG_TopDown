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
        [SerializeField] private bool _inputEnabled = true;

        private void OnEnable()
        {
            GameManager.instance.gameStateManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.instance.gameStateManager.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newGameState)
        {
            _currentGameState = newGameState;
        }

        // MARK: Input-events

        // WASD pressed
        public void OnMove(InputValue inputValue)
        {
            Vector2 vector2 = inputValue.Get<Vector2>();

            switch (_currentGameState)
            {
                case GameState.Freeroam:
                    GameManager.instance.gameEventManager.inputEvents.WASDPressed(vector2);
                    return;
                case GameState.Pause:
                    GameManager.instance.gameEventManager.inputEvents.WASDPressed(vector2);
                    return;
            }
        }

        // E-key pressed
        public void OnInteract()
        {
            if (_inputEnabled)
            {
                GameManager.instance.gameEventManager.inputEvents.InteractKeyPressed();
            }
        }

        // Q-key pressed
        public void OnCancel()
        {
            if (_inputEnabled)
            {
                GameManager.instance.gameEventManager.inputEvents.CancelKeyPressed();
            }
        }

        // R-shift held
        public void OnRun()
        {
            if (_inputEnabled)
            {
                GameManager.instance.gameEventManager.inputEvents.OnRunKeyHeld();
            }
        }

        // Esc-key pressed
        public void OnPause()
        {
            if (_inputEnabled)
            {
                GameManager.instance.gameEventManager.inputEvents.PauseKeyPressed();
            }
        }

        // J-key pressed
        public void OnJournal()
        {
            if (_inputEnabled)
            {
                GameManager.instance.gameEventManager.inputEvents.JournalKeyPressed();
            }
        }
    }
}