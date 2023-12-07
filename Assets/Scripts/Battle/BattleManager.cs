using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Arcy.Battle
{
    public class BattleManager : MonoBehaviour
    {
        private enum BattleState {playerTurn, playerMiniGame, playerAnimation, enemyTurn,};
        public static BattleManager instance;
        [SerializeField] TMP_Animated battleTextBox;
        
        private BattleState currentBattleState;
        private CanvasGroup cvGroup;
        private bool _everythingIsReset;

        private void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            CheckComponents();

            GameStateManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameStateManager.OnGameStateChanged -= OnGameStateChanged;
        }

        private void Reset()
        {
            CheckComponents();
        }

        private void CheckComponents()
        {
            if (battleTextBox == null)
            {
                battleTextBox = FindObjectOfType<TMP_Animated>();
            }
        }

        public void OnBtnPressed()
        {
            Debug.Log("Btn Pressed");
        }

        void NewTextForTextBox(string newText)
        {
            battleTextBox.ReadText(newText);
        }

        private void OnGameStateChanged(GameState newGameState)
        {
            switch (newGameState)
            {
                case GameState.Battle:
                    InitiateBattle();
                    break;
                default:
                    if (!_everythingIsReset)
                    {
                        ResetEverything();
                    }
                    break;
            }
        }

        private void InitiateBattle()
        {
            cvGroup.alpha = 1;
            print("BattleManager recognizes that it's Battle Time!");
        }

        private void ResetEverything()
        {
            cvGroup.alpha = 0;
            _everythingIsReset = true;
        }
    }
}