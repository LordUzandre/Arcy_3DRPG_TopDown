using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Arcy.Battle
{
    public class VSlice_CombatActionUI : MonoBehaviour
    {
        public GameObject panel;
        public VSlice_CombatActionBtn[] buttons;
        public GameObject descriptionPanel;
        public TextMeshProUGUI descriptionText;


        private void OnEnable()
        {
            StartCoroutine(myCoroutine());
            //Make sure waits one frame before subscribing.
            IEnumerator myCoroutine()
            {
                yield return null;
                VSlice_BattleTurnManager.instance.onNewTurn += OnNewTurn;
            }
        }

        private void OnDisable()
        {
            VSlice_BattleTurnManager.instance.onNewTurn -= OnNewTurn;
        }

        void OnNewTurn()
        {
            if (VSlice_BattleTurnManager.instance.GetCurrentTurnCharacter().team == VSlice_BattleCharacterBase.Team.Player)
            {
                DisplayCombatActions(VSlice_BattleTurnManager.instance.GetCurrentTurnCharacter());
            }
            else
            {
                DisableCombatActions();
            }
        }

        public void DisplayCombatActions(VSlice_BattleCharacterBase character)
        {
            panel.SetActive(true);

            for (int i = 0; i < buttons.Length; i++)
            {
                if (i < character.combatActions.Length)
                {
                    buttons[i].gameObject.SetActive(true);
                    buttons[i].SetCombatAction(character.combatActions[i]);
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }

        public void DisableCombatActions()
        {
            panel.SetActive(false);
            DisableCombatActionDescription();
        }

        public void SetCombatActionDescription(VSlice_CombatAction combatAction)
        {
            descriptionPanel.SetActive(true);
            descriptionText.text = combatAction.description;
        }

        public void DisableCombatActionDescription()
        {
            descriptionPanel.SetActive(false);
        }
    }
}
