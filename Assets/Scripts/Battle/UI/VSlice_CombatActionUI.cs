using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Arcy.Battle
{
    public class VSlice_CombatActionUI : MonoBehaviour
    {
        /// <summary>
        /// All player team's CombatActions UI and Buttons. The class is used by Canvas.
        /// </summary>

        [SerializeField] private GameObject _buttonPanel; // The panel containing all the CombatAction Buttons
        [SerializeField] private GameObject _descriptionPanel; // The panel containing the descriptive TextMesh

        private TextMeshProUGUI _descriptionText; // Text in the descriptionPanel
        private VSlice_CombatActionBtn[] _buttons; // All the CombatAction Buttons

        private void Start()
        {
            _descriptionText = _descriptionPanel.GetComponentInChildren<TextMeshProUGUI>();
            _buttons = _buttonPanel.GetComponentsInChildren<VSlice_CombatActionBtn>();
        }

        private void OnEnable()
        {
            StartCoroutine(myCoroutine());

            IEnumerator myCoroutine() // Wait one frame before subscribing.
            {
                yield return null;
                VSlice_BattleTurnManager.instance.onNewTurn += OnNewTurn;
            }
        }

        private void OnDisable()
        {
            VSlice_BattleTurnManager.instance.onNewTurn -= OnNewTurn;
        }

        // Called by TurnManager when a new turn has triggered.
        void OnNewTurn()
        {
            // Enable the UI if it's a player character's turn
            if (VSlice_BattleTurnManager.instance.GetCurrentTurnCharacter().team == VSlice_BattleCharacterBase.Team.Player)
            {
                DisplayCombatActions(VSlice_BattleTurnManager.instance.GetCurrentTurnCharacter());
            }
            // Otherwise disable it
            else
            {
                DisableCombatActions();
            }
        }

        // Display the requested character's available combat actions.
        private void DisplayCombatActions(VSlice_BattleCharacterBase character)
        {
            _buttonPanel.SetActive(true);

            for (int i = 0; i < _buttons.Length; i++)
            {
                if (i < character.combatActions.Length)
                {
                    _buttons[i].gameObject.SetActive(true);
                    _buttons[i].SetCombatAction(character.combatActions[i]);
                }
                else
                {
                    _buttons[i].gameObject.SetActive(false);
                }
            }
        }

        // Disable the CombatActions UI, called by CombatManager
        public void DisableCombatActions()
        {
            _buttonPanel.SetActive(false);
            DisableCombatActionDescription();
        }

        // Called by CombatActionButton when we hover over a combat action button.
        public void SetCombatActionDescription(VSlice_CombatAction combatAction)
        {
            _descriptionPanel.SetActive(true);
            _descriptionText.text = combatAction.description;
        }

        public void DisableCombatActionDescription()
        {
            _descriptionPanel.SetActive(false);
        }
    }
}
