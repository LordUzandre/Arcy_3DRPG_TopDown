using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Arcy.Battle
{
    public class CombatActionBtn : MonoBehaviour
    {
        private TextMeshProUGUI _nameText;
        private CombatActionBase _combatAction;
        private CombatActionUI _ui;
        private Vector2 _btnPosition;

        private void Awake()
        {
            _ui = FindObjectOfType<CombatActionUI>();
            _nameText = GetComponentInChildren<TextMeshProUGUI>();
            _btnPosition = gameObject.transform.localPosition;
        }

        public void SetCombatAction(CombatActionBase ca)
        {
            _combatAction = ca;
            _nameText.text = ca.displayName;
        }

        // When we click on the Btn
        public void OnClick()
        {
            PlayerCombatManager.instance.SetCurrentCombatAction(_combatAction);
        }

        //When the mouse cursor hovers over the btn
        public void OnHoverEnter()
        {
            _btnPosition = transform.position;
            _ui.SetCombatActionDescription(_combatAction, _btnPosition);
        }

        // When the mouse cursor leaves the btn
        public void OnHoverExit()
        {
            _ui.DisableCombatActionDescription();
        }
    }
}
