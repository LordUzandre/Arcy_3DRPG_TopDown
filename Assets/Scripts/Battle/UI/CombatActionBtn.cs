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
        public Button btn;
        private Vector2 _btnPosition;
        private TextMeshProUGUI _nameText;
        private CombatActionUI _combatActionUi;
        private CombatActionBase _ca;

        private void Awake()
        {
            if (_combatActionUi == null)
                _combatActionUi = FindObjectOfType<CombatActionUI>();
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
            _ca = ca;
            _nameText.text = ca.displayName;
        }

        // When we click on the Btn
        public void OnClick()
        {
            BattleManager.instance.playerCombatManager.SetCurrentCombatAction(_ca);
            _combatActionUi?.EnableCaBtns(false);
            // TODO: Add method (here or in CombatActionUI so that we go into "char-select"-mode
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
            _combatActionUi.SetCombatActionDescription(_ca, _btnPosition);
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
            _combatActionUi.DisableCombatActionDescription();
        }
    }
}
