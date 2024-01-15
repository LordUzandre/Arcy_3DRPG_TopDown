using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Arcy.Battle
{
    public class BattleTurnManager : MonoBehaviour
    {
        private List<BattleCharacterBase> turnOrder = new List<BattleCharacterBase>();
        private int curTurnOrderIndex;
        private BattleCharacterBase curTurnCharacter;

        [Header("Components")]
        public GameObject endTurnButton;

        //Singleton
        public static BattleTurnManager instance;

        public event UnityAction onNewTurn;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        // Triggered by GameManager
        public void Begin()
        {
            GenerateTurnOrder(BattleCharacterBase.Team.Player);
            NewTurn(turnOrder[0]);
        }

        void GenerateTurnOrder(BattleCharacterBase.Team startingTeam)
        {
            if (startingTeam == BattleCharacterBase.Team.Player)
            {
                turnOrder.AddRange(BattleManager.instance.playerTeam);
                turnOrder.AddRange(BattleManager.instance.enemyTeam);
            }
            else if (startingTeam == BattleCharacterBase.Team.Enemy)
            {
                turnOrder.AddRange(BattleManager.instance.enemyTeam);
                turnOrder.AddRange(BattleManager.instance.playerTeam);
            }
        }

        void NewTurn(BattleCharacterBase character)
        {
            curTurnCharacter = character;
            onNewTurn?.Invoke();

            endTurnButton.SetActive(character.team == BattleCharacterBase.Team.Player);
        }

        public void EndTurn()
        {
            curTurnOrderIndex++;

            if (curTurnOrderIndex == turnOrder.Count)
            {
                curTurnOrderIndex = 0;
            }

            // If the character is dead
            while (turnOrder[curTurnOrderIndex] == null)
            {
                curTurnOrderIndex++;

                if (curTurnOrderIndex == turnOrder.Count)
                {
                    curTurnOrderIndex = 0;
                }
            }

            NewTurn(turnOrder[curTurnOrderIndex]);
        }

        // Used by other classes to get the current active character
        public BattleCharacterBase GetCurrentTurnCharacter()
        {
            return curTurnCharacter;
        }
    }
}
