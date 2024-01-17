using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Arcy.Battle
{
    public enum TurnState { playerTeamsTurn, enemyTeamsTurn }

    public class BattleTurnManager : MonoBehaviour
    {
        private TurnState _currentTurnState;

        private List<BattleCharacterBase> _turnOrderList = new List<BattleCharacterBase>();
        private BattleCharacterBase _curTurnCharacter;
        private int _curTurnOrderIndex;

        [Header("Components")]
        [SerializeField] public Button endTurnButton; // Used by PlayerCombatManager

        // Action for a new turn
        public static Action<TurnState> onNewTurn;

        // Triggered by BattleManager
        public void Begin()
        {
            GenerateTurnOrder(BattleCharacterBase.Team.Player); // Change the overload if anybody else should start
            NewTurn(_turnOrderList[0]);
            endTurnButton.GetComponent<Button>().onClick.AddListener(EndTurn);
        }

        void GenerateTurnOrder(BattleCharacterBase.Team startingTeam)
        {
            if (startingTeam == BattleCharacterBase.Team.Player)
            {
                _turnOrderList.AddRange(BattleManager.instance.playerTeam);
                _turnOrderList.AddRange(BattleManager.instance.enemyTeam);
            }
            else if (startingTeam == BattleCharacterBase.Team.Enemy)
            {
                _turnOrderList.AddRange(BattleManager.instance.enemyTeam);
                _turnOrderList.AddRange(BattleManager.instance.playerTeam);
            }
        }

        private void NewTurn(BattleCharacterBase character)
        {
            _curTurnCharacter = character;
            //onNewTurn?.Invoke();

            switch (character.team)
            {
                // It's the player team's turn
                case (BattleCharacterBase.Team.Player):
                    if (onNewTurn != null)
                        onNewTurn(TurnState.playerTeamsTurn);
                    break;
                // It's the enemy team's turn
                case (BattleCharacterBase.Team.Enemy):
                    if (onNewTurn != null)
                        onNewTurn(TurnState.enemyTeamsTurn);
                    break;
                default:
                    Debug.LogWarning("Something Went Horribly Wrong!");
                    break;
            }

            endTurnButton.gameObject.SetActive(character.team == BattleCharacterBase.Team.Player);
        }

        // Called by PlayerCombatManager and EnemyCombatManager
        public void EndTurn()
        {
            _curTurnOrderIndex++;

            if (_curTurnOrderIndex == _turnOrderList.Count)
            {
                _curTurnOrderIndex = 0;
            }

            // If the character is dead
            while (_turnOrderList[_curTurnOrderIndex] == null)
            {
                _curTurnOrderIndex++;

                if (_curTurnOrderIndex == _turnOrderList.Count)
                {
                    _curTurnOrderIndex = 0;
                }
            }

            NewTurn(_turnOrderList[_curTurnOrderIndex]);
        }

        // Used by:
        // BattleCharacterBase
        // CombatAcitonUI
        // PlayerCombatManager
        // EnemyCombatManager
        // to get the current active character
        public BattleCharacterBase GetCurrentTurnCharacter()
        {
            return _curTurnCharacter;
        }
    }
}
