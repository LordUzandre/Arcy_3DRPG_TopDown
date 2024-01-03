using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Arcy.Battle
{
    public class VSlice_BattleTurnManager : MonoBehaviour
    {
        private List<VSlice_BattleCharacterBase> turnOrder = new List<VSlice_BattleCharacterBase>();
        private int curTurnOrderIndex;
        private VSlice_BattleCharacterBase curTurnCharacter;

        [Header("Components")]
        public GameObject endTurnButton;

        //Singleton
        public static VSlice_BattleTurnManager instance;

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

        //Triggered by GameManager
        public void Begin()
        {
            GenerateTurnOrder(VSlice_BattleCharacterBase.Team.Player);
            NewTurn(turnOrder[0]);
        }

        void GenerateTurnOrder(VSlice_BattleCharacterBase.Team startingTeam)
        {
            if (startingTeam == VSlice_BattleCharacterBase.Team.Player)
            {
                turnOrder.AddRange(VSlice_GameManager.instance.playerTeam);
                turnOrder.AddRange(VSlice_GameManager.instance.enemyTeam);
            }
            else if (startingTeam == VSlice_BattleCharacterBase.Team.Enemy)
            {
                turnOrder.AddRange(VSlice_GameManager.instance.enemyTeam);
                turnOrder.AddRange(VSlice_GameManager.instance.playerTeam);
            }
        }

        void NewTurn(VSlice_BattleCharacterBase character)
        {
            curTurnCharacter = character;
            onNewTurn?.Invoke();

            endTurnButton.SetActive(character.team == VSlice_BattleCharacterBase.Team.Player);
        }

        public void EndTurn()
        {
            curTurnOrderIndex++;

            if (curTurnOrderIndex == turnOrder.Count)
            {
                curTurnOrderIndex = 0;
            }

            //If the character is dead
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

        public VSlice_BattleCharacterBase GetCurrentCharacter()
        {
            return curTurnCharacter;
        }
    }
}
