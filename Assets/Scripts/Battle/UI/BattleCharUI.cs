using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace Arcy.Battle
{
    public class BattleCharUI : MonoBehaviour
    {
        /// <summary>
        /// This is the UI that contains all the character's stats during combat , such as name, HP-Bar etc.
        /// </summary>

        [SerializeField] private HealthBar healthbarScript;

        [Header("Turn Visuals")]
        [SerializeField] private Image _turnVisual; // Visual indicator which character's turn it is.

        private int _previousHP;
        private Vector2 _changeTMPOrgPos;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (healthbarScript == null)
                healthbarScript = GetComponentInChildren<HealthBar>();
        }
#endif

        // Called by GameManager when UI is instanced
        public void ConnectUItoNewChar(string characterName, int curHp, int maxHp)
        {
            healthbarScript.healthTMP.text = characterName;

            UpdateHealthBar(curHp, maxHp, false);
        }

        // Called by BattleCharBase when it's the current player character's turn
        public void ToggleTurnVisual(bool toggle)
        {
            _turnVisual.enabled = toggle;
        }

        // Called by BattleCharBase when the health is changed
        public void UpdateHealthBar(int curHp, int maxHp, bool shouldShowChange = true)
        {
            //Prevent the health from dropping below 0
            if (curHp <= 0)
                curHp = 0;

            healthbarScript.healthTMP.text = $"{curHp}";

            // Fill health Bar according to normalized value
            float fillAmount = (float)curHp / (float)maxHp;

            // Animate the HPBar
            if (curHp < _previousHP) // HP is shrinking
            {
                healthbarScript.healthBar.fillAmount = fillAmount;
                healthbarScript.healthBarBg.DOFillAmount(fillAmount, .5f).SetEase(Ease.InCirc);
            }
            else // HP is increasing
            {
                healthbarScript.healthBarBg.fillAmount = fillAmount;
                healthbarScript.healthBar.DOFillAmount(fillAmount, .5f).SetEase(Ease.InCirc);
            }

            StartCoroutine(myCoroutine());
            IEnumerator myCoroutine()
            {
                yield return new WaitForSeconds(1f);
                _previousHP = curHp;
            }
        }
    }
}
