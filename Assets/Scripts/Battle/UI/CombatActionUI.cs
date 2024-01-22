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
        /// All player team's CombatActions UI and Buttons. The class is put on Canvas.
        /// Connection with relevant character is made in GameManager.
        /// </summary>

        [SerializeField] private GameObject _btnPanel; // The panel containing all the CombatAction Buttons
        [SerializeField] private GameObject _descriptionPanel; // The panel containing the descriptive TextMesh

        private TextMeshProUGUI _descriptionText; // Text in the descriptionPanel
        private CombatActionBtn[] _buttons; // All the CombatAction Buttons

        private Button _currentlySelectedBtn;

        private Vector2 _descPanelOrgPos;

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
                BattleTurnManager.OnNewTurn += OnNewTurn;
                _descriptionText = _descriptionPanel.GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        private void OnDisable()
        {
            BattleTurnManager.OnNewTurn -= OnNewTurn;
        }

        // Action from BattleTurnManager when a new turn has triggered.
        private void OnNewTurn(TurnState turnState)
        {
            switch (turnState)
            {
                // Enable the UI if it's a player character's turn
                case (TurnState.playerTeamsTurn):
                    DisplayCombatActions(BattleManager.instance.battleTurnManager.GetCurrentTurnCharacter());
                    return;
                // Otherwise disable it
                case (TurnState.enemyTeamsTurn):
                    DisableCombatActions(false);
                    return;
            }
        }

        public Button PickTopBtn() // Called by PlayerCombatManager
        {
            foreach (CombatActionBtn btn in _buttons)
                if (btn.TryGetComponent<Button>(out Button hit))
                    return hit;

            return null;
        }

        // Called by PlayerCombatManager
        public void EnableCharacterBtns(bool pickOneSide, bool goodGuySide, bool enableAllBtns)
        {
            if (pickOneSide) // Enable character selection on one side
            {
                foreach (BattleCharacterBase playerUnit in BattleManager.instance.playerTeam)
                    playerUnit.selectionVisual.GetComponent<SelectionVisualBtn>().ActivateBtn(goodGuySide);

                foreach (BattleCharacterBase enemyUnit in BattleManager.instance.enemyTeam)
                    enemyUnit.selectionVisual.GetComponent<SelectionVisualBtn>().ActivateBtn(!goodGuySide);

            }
            else // Activate or deactivate all character buttons
            {
                foreach (BattleCharacterBase playerUnit in BattleManager.instance.playerTeam)
                    playerUnit.selectionVisual.GetComponent<SelectionVisualBtn>().ActivateBtn(enableAllBtns);

                foreach (BattleCharacterBase enemyUnit in BattleManager.instance.enemyTeam)
                    enemyUnit.selectionVisual.GetComponent<SelectionVisualBtn>().ActivateBtn(enableAllBtns);

            }
        }

        // Display the requested character's available combat actions.
        private void DisplayCombatActions(BattleCharacterBase character)
        {
            _btnPanel.SetActive(true);
            float spacingFloat = _btnPanel.GetComponent<VerticalLayoutGroup>().spacing;
            float btnPanelHeight = spacingFloat;

            // Activate buttons according to amount of combatActions available
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (i < character.combatActions.Length)
                {
                    _buttons[i].gameObject.SetActive(true);
                    _buttons[i].SetCombatAction(character.combatActions[i]);

                    // Set btnPanel height accrording to amount of btns
                    btnPanelHeight += (_buttons[i].GetComponent<RectTransform>().sizeDelta.y + spacingFloat);
                    _buttons[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    _buttons[i].GetComponent<Button>().interactable = false;
                    _buttons[i].gameObject.SetActive(false);
                }

                // Set the caBtnPanel Height size
                RectTransform rT = _btnPanel.GetComponent<RectTransform>();
                rT.sizeDelta = new Vector2(rT.sizeDelta.x, btnPanelHeight + spacingFloat);
            }
        }

        // Deactivate or activate ca-buttons. 
        //Called by combatActionBtn when clicking on a ca-button or returning to ca-choosing state (TODO!)
        public void EnableCaBtns(bool enableBtns)
        {
            foreach (CombatActionBtn btn in _buttons)
            {
                btn.gameObject.GetComponent<Button>().interactable = enableBtns;
            }
        }

        // Disable the CombatActions UI, called by PlayerCombatManager
        public void DisableCombatActions(bool characterChosingPhase)
        {
            //Should the UI be hidden or merely disabled
            if (characterChosingPhase) // Disable the combatActions-buttons
            {
                foreach (CombatActionBtn button in _buttons)
                    button.btn.interactable = !characterChosingPhase;
            }
            else
            {
                //Hide the UI completely
                _btnPanel.SetActive(false);
            }

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


        // Generate the correct order when entering character-choosing-state
        private void GenerateBtnChoosingOrder()
        {
            BattleCharacterBase[] allCharacters = BattleManager.instance.enemyTeam.ToArray();

            foreach (BattleCharacterBase enemyUnit in allCharacters)
            {
                Button myButton = enemyUnit.selectionVisual.GetComponent<Button>();
                Navigation navigation = myButton.navigation;
                navigation.mode = Navigation.Mode.Explicit;
                //myButton.navigation.mode = navigation.mode; 

                // allCharacters[i].selectionVisual.GetComponent<Button>().navigation.mode = Navigation.Mode.Explicit selectOnRight(allCharacters[i+1]);
                // if (allCharacters[i] == allCharacters.Length)
                // {

                // }
            }
        }

        public Button AccesCurrentlySelectedBtn()
        {
            return _currentlySelectedBtn;
        }
    }
}
