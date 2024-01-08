using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arcy.Battle
{
    public class VSlice_PlayerCombatManager : MonoBehaviour
    {
        private float selectionCheckRate = 0.1f;
        private float lastSelectionCheckTime;
        public LayerMask selectionLayerMask;

        private bool isActive;

        private VSlice_CombatAction curSelectionCombatAction;
        private VSlice_BattleCharacterBase curSelectedCharacter;

        // Selection flags
        private bool canSelectSelf;
        private bool canSelectTeam;
        private bool canSelectEnemies;

        //Singleton
        public static VSlice_PlayerCombatManager instance;

        [Header("Components")]
        public VSlice_CombatActionUI combatActionsUI;

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
                VSlice_BattleTurnManager.instance.onNewTurn += OnNewTurn;
            }
        }

        private void OnDisable()
        {
            VSlice_BattleTurnManager.instance.onNewTurn -= OnNewTurn;
        }

        private void OnNewTurn()
        {
            if (VSlice_BattleTurnManager.instance.GetCurrentTurnCharacter().team == VSlice_BattleCharacterBase.Team.Player)
            {
                EnablePlayerCombat();
            }
            else
            {
                DisablePlayerCombat();
            }
        }

        private void EnablePlayerCombat()
        {
            curSelectedCharacter = null;
            curSelectionCombatAction = null;
            isActive = true;
        }

        private void DisablePlayerCombat()
        {
            isActive = false;
        }

        private void Update()
        {
            if (!isActive || curSelectionCombatAction == null)
                return;

            if (Time.time - lastSelectionCheckTime > selectionCheckRate)
            {
                lastSelectionCheckTime = Time.time;
                SelectionCheck();
            }

            if (Mouse.current.leftButton.isPressed && curSelectedCharacter != null)
            {
                CastCombatAction();
            }
        }

        //See what we're hovering over
        private void SelectionCheck()
        {

            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 999, selectionLayerMask))
            {
                VSlice_BattleCharacterBase character = hit.collider.GetComponentInParent<VSlice_BattleCharacterBase>();

                if (curSelectedCharacter != null && curSelectedCharacter == character)
                {
                    return;
                }

                if (canSelectSelf && character == VSlice_BattleTurnManager.instance.GetCurrentTurnCharacter())
                {
                    SelectCharacter(character);
                    return;
                }
                else if (canSelectTeam && character.team == VSlice_BattleCharacterBase.Team.Player)
                {
                    SelectCharacter(character);
                    return;
                }
                else if (canSelectEnemies && character.team == VSlice_BattleCharacterBase.Team.Enemy)
                {
                    SelectCharacter(character);
                    return;
                }
            }

            UnSelectCharacter();
        }

        void CastCombatAction()
        {
            VSlice_BattleTurnManager.instance.GetCurrentTurnCharacter().CastCombatAction(curSelectionCombatAction, curSelectedCharacter);
            curSelectionCombatAction = null;

            UnSelectCharacter();
            DisablePlayerCombat();
            combatActionsUI.DisableCombatActions();
            VSlice_BattleTurnManager.instance.endTurnButton.SetActive(false);

            Invoke(nameof(NextTurnDelay), 1.0f);
        }

        private void NextTurnDelay()
        {
            VSlice_BattleTurnManager.instance.EndTurn();
        }

        //CAlled when we hover over a character
        private void SelectCharacter(VSlice_BattleCharacterBase character)
        {
            UnSelectCharacter();
            curSelectedCharacter = character;
            character.ToggleSelectionVisual(true);
        }

        //Called when we stop hovering over a character.
        private void UnSelectCharacter()
        {
            if (curSelectedCharacter == null) return;

            curSelectedCharacter.ToggleSelectionVisual(false);
            curSelectedCharacter = null;
        }

        //Called when we click on a ombat action in the UI panel.
        public void SetCurrentCombatAction(VSlice_CombatAction combatAction)
        {
            curSelectionCombatAction = combatAction;

            canSelectSelf = false;
            canSelectTeam = false;
            canSelectEnemies = false;

            if (combatAction as VSlice_CombatActionMelee || combatAction as VSlice_CombatActionRanged)
            {
                canSelectEnemies = true;
            }
            else if (combatAction as VSlice_CombatActionHeal)
            {
                canSelectSelf = true;
                canSelectTeam = true;
            }
            else if (combatAction as VSlice_CombatActionEffect)
            {
                canSelectSelf = (combatAction as VSlice_CombatActionEffect).canEffectSelf;
                canSelectTeam = (combatAction as VSlice_CombatActionEffect).canEffectTeam;
                canSelectEnemies = (combatAction as VSlice_CombatActionEffect).canEffectEnemy;
            }
        }
    }
}
