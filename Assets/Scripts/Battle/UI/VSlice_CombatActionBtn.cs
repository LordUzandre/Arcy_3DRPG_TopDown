using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Arcy.Battle
{
    public class VSlice_CombatActionBtn : MonoBehaviour
    {
        public TextMeshProUGUI nameText;

        private VSlice_CombatAction combatAction;
        private VSlice_CombatActionUI ui;

        private void Awake()
        {
            ui = FindObjectOfType<VSlice_CombatActionUI>();
        }

        public void SetCombatAction(VSlice_CombatAction ca)
        {
            combatAction = ca;
            nameText.text = ca.displayName;
        }

        public void OnClick()
        {
            VSlice_PlayerCombatManager.instance.SetCurrentCombatAction(combatAction);
        }

        public void OnHoverEnter()
        {
            ui.SetCombatActionDescription(combatAction);
        }

        public void OnHoverExit()
        {
            ui.DisableCombatActionDescription();
        }

        private void OnValidate()
        {
            nameText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}
