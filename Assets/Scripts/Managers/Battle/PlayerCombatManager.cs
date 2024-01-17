using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Arcy.Battle
{
    public enum PlayerTurnState { defauultStartState, chooseCombatAction, chooseEnemy, choosePlayerChar, choosePlayerTeamChar, chooseItem, chooseAnyChar, enemiesTurn, battleIsOver }

    public class PlayerCombatManager : MonoBehaviour
    {
        /// <summary>
        /// This manager handles the combat during the player's turn
        /// </summary>

        // Private:
        [Header("Components")]
        [SerializeField] private LayerMask _selectionLayerMask; // LayerMask
        [SerializeField] private CombatActionUI combatActionsUI; // PlayerTeam CombatActions UI

        private CombatActionBase _curSelectionCombatAction; // Current selected combatAction
        private BattleCharacterBase _curSelectedCharacter; // Current selected character

        private PlayerTurnState _previousTurnState;

#if UNITY_EDITOR
        private void OnValidate()
        {
            //Set the LayerMask to the appropriate layer
            if (_selectionLayerMask == LayerMask.GetMask("Nothing"))
                _selectionLayerMask = LayerMask.GetMask("Player_combat");
        }
#endif

        private void OnEnable()
        {
            StartCoroutine(WaitToSubscribe());

            IEnumerator WaitToSubscribe()
            {
                yield return null;
                BattleTurnManager.onNewTurn += OnNewTurn;
            }
        }

        private void OnDisable()
        {
            BattleTurnManager.onNewTurn -= OnNewTurn;
        }

        // Called when a new turn has triggered.
        private void OnNewTurn(TurnState turnState)
        {
            switch (turnState)
            {
                case (TurnState.playerTeamsTurn):
                    NewTurnState(PlayerTurnState.chooseCombatAction);
                    return;
            }
        }

        // Allow the player to select combat actions and select targets.
        // Called at the beginning of player's turn or (TODO!) if the character cancels their current action
        private void EnablePlayerCombat()
        {
            //_curSelectedCharacter = null;
            _curSelectionCombatAction = null;

            combatActionsUI.EnableCaBtns(true); // enable combatAction Buttons
            EventSystem.current.SetSelectedGameObject(combatActionsUI.PickTopBtn()); // Pick the top btn in UI
        }

        // Called when we click on a target character with a combat action selected.
        public void CastCombatAction(BattleCharacterBase character)
        {
            //_curSelectedCharacter = character;
            BattleManager.instance.battleTurnManager.GetCurrentTurnCharacter().CastCombatAction(_curSelectionCombatAction, character);
            _curSelectionCombatAction = null;

            NewTurnState(PlayerTurnState.enemiesTurn);
        }

        // Called when we click on a combat action in the UI panel.
        public void SetCurrentCombatAction(CombatActionBase combatAction)
        {
            _curSelectionCombatAction = combatAction;

            if (combatAction as CombatAction_Melee || combatAction as CombatAction_Ranged)
            {
                NewTurnState(PlayerTurnState.chooseEnemy);
            }
            else if (combatAction as CombatAction_Heal)
            {
                NewTurnState(PlayerTurnState.choosePlayerTeamChar);
            }
            else if (combatAction as CombatAction_Effect)
            {
                NewTurnState(PlayerTurnState.chooseAnyChar);
            }
        }

        public void NewTurnState(PlayerTurnState currentPlayerTurnState)
        {
            if (_previousTurnState == currentPlayerTurnState)
                return;

            _previousTurnState = currentPlayerTurnState;

            switch (currentPlayerTurnState)
            {
                case (PlayerTurnState.chooseCombatAction):
                    EnablePlayerCombat();
                    return;

                case (PlayerTurnState.chooseEnemy):
                    combatActionsUI.EnableCharacterBtns(true, false, false); // Enable to choose an enemy character
                    EventSystem.current.SetSelectedGameObject(GetTopEnemyCharacter()); // mark first enemy team member TODO: check whether they are dead!
                    return;

                case (PlayerTurnState.choosePlayerTeamChar):
                    combatActionsUI.EnableCharacterBtns(true, true, false); // Enable full Player-team characters choice
                    EventSystem.current.SetSelectedGameObject(BattleManager.instance.playerTeam[0].selectionVisual); // mark first player team member TODO: Check whether they are dead!
                    return;

                case (PlayerTurnState.choosePlayerChar):
                    BattleManager.instance.battleTurnManager.GetCurrentTurnCharacter().selectionVisual.GetComponent<SelectionVisualBtn>().ActivateBtn(true); // Enable current Player-team character choice
                    return;

                case (PlayerTurnState.chooseAnyChar):
                    combatActionsUI.EnableCharacterBtns(false, false, true); // Enables to choose ANY currently active character
                    return;

                case (PlayerTurnState.enemiesTurn):
                    combatActionsUI?.DisableCombatActions(false); // Disable the CombatActionsUI
                    combatActionsUI.EnableCharacterBtns(false, true, false);
                    BattleManager.instance.battleTurnManager.endTurnButton.gameObject.SetActive(false); // Hide the EndTurnBtn
                    Invoke(nameof(NextTurnDelay), 1.5f); // Invoke NextTurn after a short delay
                    return;

                default:
                    Debug.Log("Oops, like you didn't plan for that yet, Dumbo!");
                    return;
            }
        }

        private GameObject GetTopEnemyCharacter()
        {
            for (int i = 0; i < BattleManager.instance.enemyTeam.Count; i++)
                if (BattleManager.instance.enemyTeam[i] != null)
                    return BattleManager.instance.enemyTeam[i].selectionVisual;

            // If no non-null enemy GameObject is found, return null
            Debug.LogWarning("No Enemy was found!");
            return null;
        }

        private GameObject GetTopPlayerCharacter()
        {
            for (int i = 0; i < BattleManager.instance.playerTeam.Count; i++)
                if (BattleManager.instance.playerTeam[i] != null)
                    return BattleManager.instance.playerTeam[i].gameObject;

            // If no non-null playerTeam GameObject is found, return null
            Debug.LogWarning("No Team Player Char was found!");
            return null;
        }

        // Initiate the next character's turn.
        private void NextTurnDelay()
        {
            BattleManager.instance.battleTurnManager.EndTurn();
        }
    }
}
