using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Arcy.Dialogue;

//This script should handle all the visuals for DialogueUI;
public class DialogueUI : MonoBehaviour
{
    [SerializeField] public TMP_Animated dialogueText;
    [SerializeField] Image dialogueBox;
    [SerializeField] Image dialogueArrow;
    CanvasGroup cvGroup;

    //public bool _currentlyTyping;

    private void OnEnable()
    {
        CheckComponents();
        cvGroup.alpha = 0;
    }

    private void CheckComponents()
    {
        if (dialogueText == null)
            dialogueText = GetComponentInChildren<TMP_Animated>();

        if (dialogueBox == null)
            dialogueBox = GetComponentInChildren<Image>();

        if (dialogueArrow == null)
            dialogueArrow = GetComponentInChildren<Image>();

        if (cvGroup == null)
            cvGroup = GetComponent<CanvasGroup>();
    }

    public void FadeUI(bool show, float time, float delay)
    {
        dialogueText.text = string.Empty;

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(delay);

        // Fade in or out depending on bool
        sequence.Append(cvGroup.DOFade(show ? 1 : 0, time));

        if (show)
        {
            DialogueManager.Instance.dialogueIndex = 0;
            //pop the size of the UI
            sequence.Join(cvGroup.transform.DOScale(0, time * 2).From().SetEase(Ease.OutBack));

            //Start typing the text after the UI is faded in.
            sequence.AppendCallback(() => TypeOutDialogueText(0));
        }
    }

    public void TypeOutDialogueText(int index)
    {
        dialogueText.ReadText(DialogueManager.Instance.dialogueBlock[index]);
        //_currentlyTyping = true;
    }
}