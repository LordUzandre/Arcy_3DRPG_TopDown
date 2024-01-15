using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Battle
{
    public class HealthBar : MonoBehaviour
    {
        [Header("Bar")]
        [SerializeField] public Image healthBar;
        [SerializeField] public TextMeshProUGUI healthTMP;

        [Header("HP Changes")]
        [SerializeField] public TextMeshProUGUI changeTMP;
        [SerializeField] private Color _red = new Color(132, 24, 24);
        [SerializeField] private Color _green = new Color(60, 214, 83);


    }
}
