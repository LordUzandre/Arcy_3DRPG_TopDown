using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Arcy.Battle
{
    public class BattleCharUI : MonoBehaviour
    {
        /// <summary>
        /// This is the UI that contains all the character's stats during combat , such as name, HP-Bar etc.
        /// </summary>

        [Header("Character Name")]
        [SerializeField] private TextMeshProUGUI _characterNameText; // TextMesh that's set by CharacterBase

        [Header("Turn Visuals")]
        [SerializeField] private Image _turnVisual; // Visual indicator which character's turn it is.

        [Header("HP")]
        [SerializeField] private Image _healthBar; // healthBar
        [SerializeField] private TextMeshProUGUI _healthText; // TextMesh that shows character's hp

        // Called by GameManager when UI is instanced
        public void ConnectUItoNewChar(string characterName, int curHp, int maxHp)
        {
            _characterNameText.text = characterName;
            UpdateHealthBar(curHp, maxHp);
        }

        // Called by BattleCharBase when it's the current player character's turn
        public void ToggleTurnVisual(bool toggle)
        {
            _turnVisual.enabled = toggle;
        }

        // Called by BattleCharBase when the health is changed
        public void UpdateHealthBar(int curHp, int maxHp)
        {
            _healthText.text = $"{curHp}";

            // Fill health Bar according to normalized value
            _healthBar.fillAmount = (float)curHp / (float)maxHp;
        }
    }
}
