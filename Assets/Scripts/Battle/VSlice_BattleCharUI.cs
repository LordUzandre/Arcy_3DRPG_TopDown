using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Arcy.Battle
{
    public class VSlice_BattleCharUI : MonoBehaviour
    {
        public TextMeshProUGUI characterNameText;
        public Image healthFill;
        public TextMeshProUGUI healthText;
        public Image turnVisual;

        private void Update()
        {
            transform.forward = transform.position - UnityEngine.Camera.main.transform.position;
        }

        public void ToggleTurnVisual(bool toggle)
        {
            turnVisual.gameObject.SetActive(toggle);
        }

        public void SetCharacterNameText(string characterName)
        {
            characterNameText.text = characterName;
        }

        public void UpdateHealthBar(int curHP, int maxHP)
        {
            healthText.text = $"{curHP} / {maxHP}";
            healthFill.fillAmount = (float)curHP / (float)maxHP;
        }
    }
}
