using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] public TMPro.TMP_Animated dialogueText;
    [Header("Sprites")]
    [SerializeField] public UnityEngine.UI.Image dialogueBox;
    [SerializeField] public UnityEngine.UI.Image dialogueArrow;

    private void OnEnable()
    {
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
