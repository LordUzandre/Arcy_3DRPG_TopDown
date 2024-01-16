using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace Arcy.Battle
{
    public class CombatActionBtn : EventTrigger //MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISelectHandler
    {
        [SerializeField] public Button btn;
        private TextMeshProUGUI _nameText;
        private CombatActionUI _ui;
        private Vector2 _btnPosition;
        private CombatActionBase _combatAction;

        private void Awake()
        {
            if (_ui == null)
                _ui = FindObjectOfType<CombatActionUI>();
            if (_nameText == null)
                _nameText = GetComponentInChildren<TextMeshProUGUI>();
            if (btn == null)
                btn = GetComponent<Button>();

            _btnPosition = gameObject.transform.localPosition;
        }

        private void OnEnable()
        {
            // Subscribe to OnClick whenever we click on this button
            btn?.onClick.AddListener(OnClick);
        }

        // Sets the appropriate combatAction. Called by CombatActionUI based on BattleBaseCharacter
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

        // When mouse clicker enters a new btn
        public override void OnPointerEnter(PointerEventData eventData)
        {
            Selected();
        }

        // When the mouse cursor hovers over the btn
        public override void OnSelect(BaseEventData eventData)
        {
            Selected();
        }

        private void Selected()
        {
            _btnPosition = transform.position;
            _ui.SetCombatActionDescription(_combatAction, _btnPosition);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            // when mouse pointer exits a btn
            OnHoverExit();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            // when the selector leaves the current btn
            OnHoverExit();
        }

        // When the mouse cursor leaves the btn
        private void OnHoverExit()
        {
            _ui.DisableCombatActionDescription();
        }
    }
}
