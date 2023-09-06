using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] public TMPro.TextMeshProUGUI nameText;
    [SerializeField] public TMPro.TMP_Animated dialogueText;
    [Header("Sprites")]
    [SerializeField] public UnityEngine.UI.Image dialogueBox;
    [SerializeField] public UnityEngine.UI.Image dialogueArrow;

    private void Start()
    {
        if (nameText == null)
        {
            nameText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }

        if (dialogueText == null)
        {
            dialogueText = GetComponentInChildren<TMPro.TMP_Animated>();
        }

        if (dialogueBox == null)
        {
            dialogueBox = GetComponentInChildren<UnityEngine.UI.Image>();
        }

        if (dialogueArrow == null)
        {
            dialogueArrow = GetComponentInChildren<UnityEngine.UI.Image>();
        }
    }

}
