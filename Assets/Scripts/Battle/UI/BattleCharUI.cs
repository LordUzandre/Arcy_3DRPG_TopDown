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
        [SerializeField] private TextMeshProUGUI characterNameText; // TextMesh that's set by CharacterBase

        [Header("Turn Visuals")]
        [SerializeField] private Image _turnVisual; // Visual indicator which character's turn it is.

        [Header("HP")]
        [SerializeField] private Image healthFill; // healthBar
        [SerializeField] private TextMeshProUGUI healthText; // TextMesh that shows character's hp

        // Called by GameManager when UI is instanced
        public void ConnectUItoNewChar(string characterName, int curHp, int maxHp)
        {
            characterNameText.text = characterName;
            UpdateHealthBar(curHp, maxHp);
        }

        // Called by BattleCharBase when it's the current player character's turn
        public void ToggleTurnVisual(bool toggle)
        {
            _turnVisual.gameObject.SetActive(toggle);
        }

        // Called by BattleCharBase when the health is changed
        public void UpdateHealthBar(int curHp, int maxHp)
        {
            healthText.text = $"{curHp}";

            // Fill health Bar according to normalized value
            healthFill.fillAmount = (float)curHp / (float)maxHp;
        }
    }
}
