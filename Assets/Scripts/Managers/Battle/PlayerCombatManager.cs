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
    public enum PlayerTurnState
    {
        defaultStartState,
        chooseCombatAction,
        chooseEnemy,
        chooseSingleChar,
        choosePlayerTeamChar,
        chooseItem,
        chooseAnyChar,
        enemiesTurn,
        battleIsOver,
        playerAttackMiniGame,
        enemyAttackDefGame
    }

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
                BattleTurnManager.OnNewTurn += OnNewTurn;
            }
        }

        private void OnDisable()
        {
            BattleTurnManager.OnNewTurn -= OnNewTurn;
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

            // TODO: Add a method to check which type of input we are using
            combatActionsUI.PickTopBtn().Select(); // Highlight the top btn in UI
        }

        // Called when we click on a target character with a combat action selected.
        public void CastCombatAction(BattleCharacterBase character)
        {
            //_curSelectedCharacter = character;
            BattleManager.instance.battleTurnManager.GetCurrentTurnCharacter().CastCombatAction(_curSelectionCombatAction, character);
            _curSelectionCombatAction = null;

            NewTurnState(PlayerTurnState.enemiesTurn);
        }

        // Called by CombactionBTN when we click on a combat action in the UI panel.
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
                    EnablePlayerCombat(); // Turn on CombatActionsUI
                    combatActionsUI?.EnableCharacterBtns(false, false, false); // Deactivate all characterBtns
                    return;

                case (PlayerTurnState.chooseEnemy):
                    combatActionsUI?.EnableCharacterBtns(true, false, false); // Enable to choose an enemy character

                    // TODO: Add method to check what type of input we are using

                    MarkFirstLivingCharacterInUi(BattleManager.instance.enemyTeam).Select(); // if we are using keyboard or gamepad => mark first enemy team member
                    return;

                case (PlayerTurnState.choosePlayerTeamChar):
                    combatActionsUI?.EnableCharacterBtns(true, true, false); // Enable full Player-team characters choice

                    // TODO: Add method to check what type of input we are using

                    MarkFirstLivingCharacterInUi(BattleManager.instance.playerTeam).Select(); // if we are using keyboard or gamepad => mark first enemy team member
                    return;

                case (PlayerTurnState.chooseSingleChar):
                    BattleManager.instance.battleTurnManager.GetCurrentTurnCharacter().selectionVisual.GetComponent<SelectionVisualBtn>().ActivateBtn(true); // Enable current Player-team character choice
                    return;

                case (PlayerTurnState.chooseAnyChar):
                    combatActionsUI?.EnableCharacterBtns(false, false, true); // Enables to choose ANY currently active character
                    return;

                case (PlayerTurnState.enemiesTurn):
                    combatActionsUI?.EnableCharacterBtns(false, false, false); // Deactivate all characterBtns
                    combatActionsUI?.DisableCombatActions(false); // Disable the CombatActionsUI
                    combatActionsUI?.EnableCharacterBtns(false, true, false);
                    BattleManager.instance.battleTurnManager.endTurnButton.gameObject.SetActive(false); // Hide the EndTurnBtn
                    StartCoroutine(NextTurnDelay()); // Invoke NextTurn after a short delay
                    return;

                    IEnumerator NextTurnDelay()
                    {
                        yield return new WaitForSeconds(1.5f);
                        // Initiate the next character's turn after a short wait
                        BattleManager.instance.battleTurnManager.EndTurn();
                    }

                case (PlayerTurnState.defaultStartState):
                    return;

                default:
                    Debug.Log("Oops, like you didn't plan for that yet, Dumbo!");
                    return;
            }
        }

        private Button MarkFirstLivingCharacterInUi(List<BattleCharacterBase> whichSide)
        {
            foreach (BattleCharacterBase enemyUnit in whichSide)
                if (enemyUnit != null)
                    return enemyUnit.selectionVisual.GetComponent<Button>();

            // If no non-null enemy GameObject is found, return null
            Debug.LogWarning("No Character was found!");
            return null;
        }
    }
}
