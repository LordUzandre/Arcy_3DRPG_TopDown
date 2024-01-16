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

        [Header("Character Name")]
        [SerializeField] private TextMeshProUGUI _characterNameText; // TextMesh that's set by CharacterBase

        [Header("Turn Visuals")]
        [SerializeField] private Image _turnVisual; // Visual indicator which character's turn it is.

        [Header("HP")]
        [SerializeField] private Image _healthBar; // healthBar
        [SerializeField] private TextMeshProUGUI _healthText; // TextMesh that shows character's hp

        [Header("HP Changes")]
        [SerializeField] private TextMeshProUGUI _changeTMP; // TMP that's used to show changes in HP
        [SerializeField] private Color _red = new Color(132, 24, 24);
        [SerializeField] private Color _green = new Color(60, 214, 83);

        private int _previousHP;
        private Vector2 _changeTMPOrgPos;

        // Called by GameManager when UI is instanced
        public void ConnectUItoNewChar(string characterName, int curHp, int maxHp)
        {
            _characterNameText.text = characterName;

            // Set changeTMP
            _previousHP = curHp;
            _changeTMP.alpha = 0;
            _changeTMPOrgPos = _changeTMP.transform.localPosition;

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

            _healthText.text = $"{curHp}";

            // Fill health Bar according to normalized value
            float fillAmount = (float)curHp / (float)maxHp;
            _healthBar.DOFillAmount(fillAmount, .25f).SetEase(Ease.OutCirc);

            // if (shouldShowChange)
            //     ShowChanges(curHp);
        }

        // private void ShowChanges(int curHp)
        // {
        //     //Show changes in HP
        //     if (_previousHP > curHp) // the HP is decreasing (damage)
        //     {
        //         _changeTMP.text = (_previousHP - curHp).ToString();
        //         _changeTMP.color = _red;
        //     }
        //     else if (_previousHP < curHp) // the HP is increasing (healing)
        //     {
        //         _changeTMP.text = (curHp - _previousHP).ToString();
        //         _changeTMP.color = _green;
        //     }

        //     Sequence sequence = DOTween.Sequence();

        //     _changeTMP.transform.localPosition = _changeTMPOrgPos;
        //     _changeTMP.alpha = 1;

        //     if (sequence.IsPlaying())
        //         DOTween.Sequence().Restart();

        //     // sequence.Append(_changeTMP.DOFade(1, 0.2f)).SetEase(Ease.OutExpo); // Fade In
        //     sequence.Insert(0.2f, _changeTMP.transform.DOMoveY(_changeTMP.transform.position.y + 32, 2f).SetEase(Ease.OutElastic)); // Slide up along y-axis
        //     sequence.Insert(0.2f, _changeTMP.DOFade(0f, 1f)); // Fade Out
        // }
    }
}
