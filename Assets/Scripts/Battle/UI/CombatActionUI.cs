using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Arcy.Battle
{
    public class CombatActionUI : MonoBehaviour
    {
        /// <summary>
        /// All player team's CombatActions UI and Buttons. The class is used by Canvas.
        /// Connection with relevant character is made in GameManager.
        /// </summary>

        [SerializeField] private GameObject _btnPanel; // The panel containing all the CombatAction Buttons
        [SerializeField] private GameObject _descriptionPanel; // The panel containing the descriptive TextMesh

        private TextMeshProUGUI _descriptionText; // Text in the descriptionPanel
        private CombatActionBtn[] _buttons; // All the CombatAction Buttons

        private Vector2 _descPanelOrgPos;

        //float _btnPanelHeight;

        private void Start()
        {
            _buttons = _btnPanel.GetComponentsInChildren<CombatActionBtn>();
        }

        private void OnEnable()
        {
            StartCoroutine(myCoroutine());

            IEnumerator myCoroutine() // Wait one frame before subscribing to avoid error message.
            {
                yield return null;
                BattleTurnManager.instance.onNewTurn += OnNewTurn;
                _descriptionText = _descriptionPanel.GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        private void OnDisable()
        {
            BattleTurnManager.instance.onNewTurn -= OnNewTurn;
        }

        // Called by TurnManager when a new turn has triggered.
        void OnNewTurn()
        {
            // Enable the UI if it's a player character's turn
            if (BattleTurnManager.instance.GetCurrentTurnCharacter().team == BattleCharacterBase.Team.Player)
            {
                DisplayCombatActions(BattleTurnManager.instance.GetCurrentTurnCharacter());
            }
            // Otherwise disable it
            else
            {
                DisableCombatActions();
            }
        }

        public GameObject PickTopBtn(bool chooseBtn = true)
        {
            if (chooseBtn) // Select the top Btn, called by DisplayCombatActions()
            {
                return _buttons[0].gameObject;
            }
            else
            {
                return null;
            }
        }

        // Display the requested character's available combat actions.
        private void DisplayCombatActions(BattleCharacterBase character)
        {
            _btnPanel.SetActive(true);
            float spacingFloat = _btnPanel.GetComponent<VerticalLayoutGroup>().spacing;
            float _btnPanelHeight = spacingFloat;

            // Activate buttons according to amount of combatActions available
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (i < character.combatActions.Length)
                {
                    _buttons[i].gameObject.SetActive(true);
                    _buttons[i].SetCombatAction(character.combatActions[i]);

                    // Set btnPanel height accrording to amount of btns
                    _btnPanelHeight += (_buttons[i].GetComponent<RectTransform>().sizeDelta.y + spacingFloat);
                }
                else
                {
                    _buttons[i].gameObject.SetActive(false);
                }

                // Set the caBtnPanel Height size
                RectTransform rT = _btnPanel.GetComponent<RectTransform>();
                rT.sizeDelta = new Vector2(rT.sizeDelta.x, _btnPanelHeight + spacingFloat);
            }
        }

        // Disable the CombatActions UI, called by PlayerCombatManager
        public void DisableCombatActions()
        {
            _btnPanel.SetActive(false);
            DisableCombatActionDescription();
        }

        // Called by CombatActionButton when we hover over a combat action button.
        public void SetCombatActionDescription(CombatActionBase combatAction, Vector2 btnPosition)
        {
            _descriptionPanel.SetActive(true);

            if (_descriptionText != null)
                _descriptionText.text = combatAction.description;

            // move the DescriptionPanel to the selected CombatAction
            _descriptionPanel.transform.position = new Vector2(_descriptionPanel.transform.position.x, btnPosition.y);
        }

        public void DisableCombatActionDescription()
        {
            _descriptionPanel.SetActive(false);
        }
    }
}
