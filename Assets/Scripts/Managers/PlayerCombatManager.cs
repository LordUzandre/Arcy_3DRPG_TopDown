using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arcy.Battle
{
    public class PlayerCombatManager : MonoBehaviour
    {
        /// <summary>
        /// This manager handles the combat during the player's turn
        /// </summary>

        // Public:
        public static PlayerCombatManager instance; // Singleton

        // Private:
        [Header("Components")]
        [SerializeField] private LayerMask _selectionLayerMask; // LayerMask
        [SerializeField] private CombatActionUI combatActionsUI; // PlayerTeam CombatActions UI

        private CombatActionBase _curSelectionCombatAction; // Current selected combatAction
        private BattleCharacterBase _curSelectedCharacter; // Current selected character
        private bool _isActive; // Whether PlayerCombat is active or not

        // Selection flags for the currently selected combatAction
        private bool _canSelectSelf;
        private bool _canSelectTeam;
        private bool _canSelectEnemies;

        // Selection Raycast variables
        private float _selectionCheckRate = 0.1f;
        private float _lastSelectionCheckTime;

#if UNITY_EDITOR
        private void OnValidate()
        {
            //Set the LayerMask to the appropriate layer
            if (_selectionLayerMask == LayerMask.GetMask("Nothing"))
                _selectionLayerMask = LayerMask.GetMask("Player_combat");
        }
#endif

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

        private void OnEnable()
        {
            StartCoroutine(WaitToSubscribe());

            IEnumerator WaitToSubscribe()
            {
                yield return null;
                BattleTurnManager.instance.onNewTurn += OnNewTurn;
            }
        }

        private void OnDisable()
        {
            BattleTurnManager.instance.onNewTurn -= OnNewTurn;
        }

        // Called when a new turn has triggered.
        private void OnNewTurn()
        {
            // Enable player combat if it's our turn.
            if (BattleTurnManager.instance.GetCurrentTurnCharacter().team == BattleCharacterBase.Team.Player)
            {
                EnablePlayerCombat();
            }
            // Disable it otherwise.
            else
            {
                DisablePlayerCombat();
            }
        }

        // Allow the player to select combat actions and select targets.
        private void EnablePlayerCombat()
        {
            _curSelectedCharacter = null;
            _curSelectionCombatAction = null;
            _isActive = true;
        }

        private void DisablePlayerCombat()
        {
            _isActive = false;
        }

        private void Update()
        {
            // Only run update function if combat is enabled.
            if (!_isActive || _curSelectionCombatAction == null)
                return;

            // Check to see what the mouse is hovering over.
            if (Time.time - _lastSelectionCheckTime > _selectionCheckRate)
            {
                _lastSelectionCheckTime = Time.time;
                SelectionCheck();
            }

            // When we click, cast the combat action.
            if (Mouse.current.leftButton.isPressed && _curSelectedCharacter != null)
            {
                CastCombatAction();
            }
        }

        //See what we're hovering over
        private void SelectionCheck()
        {

            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 999, _selectionLayerMask))
            {
                BattleCharacterBase character = hit.collider.GetComponentInParent<BattleCharacterBase>();

                if (_curSelectedCharacter != null && _curSelectedCharacter == character)
                {
                    return;
                }

                if (_canSelectSelf && character == BattleTurnManager.instance.GetCurrentTurnCharacter())
                {
                    SelectCharacter(character);
                    return;
                }
                else if (_canSelectTeam && character.team == BattleCharacterBase.Team.Player)
                {
                    SelectCharacter(character);
                    return;
                }
                else if (_canSelectEnemies && character.team == BattleCharacterBase.Team.Enemy)
                {
                    SelectCharacter(character);
                    return;
                }
            }

            UnSelectCharacter();
        }

        // Called when we click on a target character with a combat action selected.
        void CastCombatAction()
        {
            BattleTurnManager.instance.GetCurrentTurnCharacter().CastCombatAction(_curSelectionCombatAction, _curSelectedCharacter);
            _curSelectionCombatAction = null;

            UnSelectCharacter();
            DisablePlayerCombat();
            combatActionsUI?.DisableCombatActions();
            BattleTurnManager.instance.endTurnButton.SetActive(false);

            Invoke(nameof(NextTurnDelay), 1.0f);
        }

        // Initiate the next character's turn.
        private void NextTurnDelay()
        {
            BattleTurnManager.instance.EndTurn();
        }

        //Called when we hover over a character
        private void SelectCharacter(BattleCharacterBase character)
        {
            UnSelectCharacter();
            _curSelectedCharacter = character;
            character.ToggleSelectionVisual(true);
        }

        //Called when we stop hovering over a character.
        private void UnSelectCharacter()
        {
            if (_curSelectedCharacter == null) return;

            _curSelectedCharacter.ToggleSelectionVisual(false);
            _curSelectedCharacter = null;
        }

        //Called when we click on a ombat action in the UI panel.
        public void SetCurrentCombatAction(CombatActionBase combatAction)
        {
            _curSelectionCombatAction = combatAction;

            _canSelectSelf = false;
            _canSelectTeam = false;
            _canSelectEnemies = false;

            if (combatAction as CombatAction_Melee || combatAction as CombatAction_Ranged)
            {
                _canSelectEnemies = true;
            }
            else if (combatAction as CombatAction_Heal)
            {
                _canSelectSelf = true;
                _canSelectTeam = true;
            }
            else if (combatAction as CombatAction_Effect)
            {
                _canSelectSelf = (combatAction as CombatAction_Effect).canEffectSelf;
                _canSelectTeam = (combatAction as CombatAction_Effect).canEffectTeam;
                _canSelectEnemies = (combatAction as CombatAction_Effect).canEffectEnemy;
            }
        }
    }
}
