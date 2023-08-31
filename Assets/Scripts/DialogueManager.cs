//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Cinemachine;
using UnityEngine.Rendering;
using Random = System.Random;

public class DialogueManager : MonoBehaviour
{
    //singleton
    public static DialogueManager instance;

    [Header("Dialogue Canvas Group")]
    public CanvasGroup canvasGroup;
    [Space]
    [Header("Cameras")]
    public GameObject gameCam;
    public GameObject dialogueCam;
    [SerializeField] public CinemachineTargetGroup targetGroup;
    [Space]
    [Header("Post-proccessing")]
    public UnityEngine.Rendering.Volume dialogueDof;

    //[HideInInspector] public TextMeshProUGUI speakerNameText;
    //[HideInInspector] public TextMeshProUGUI dialogueText;
    [HideInInspector] public Interactible currentInteractible;
    public TMP_Animated dialogueText;

    [Header("Bools")]
    public bool currentlyInDialogue = false;
    public bool nextDialogue = false;
    public bool canExit = false;
    private int dialogueIndex = 0;

    private void Awake()
    {
        if (instance == null) { instance = this; } else { Destroy(this); }
    }

    private void Start()
    {
        if (canvasGroup == null)
        {
            GameObject.FindGameObjectWithTag("DialogueUI").GetComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0;
        dialogueText.onDialogueFinish.AddListener(() => FinishDialogue());
    }

    public void StartDialogue()
    {
        currentInteractible = PlayerManager.instance.interactible;

        if (currentInteractible.isNPC)
        {
            currentInteractible.TurnToPlayer(transform.position);
        }

        //camera settings
        targetGroup.m_Targets[1].target = currentInteractible.transform;

        //ui.SetNameTextAndColor(); //FIX LATER
        currentlyInDialogue = true;
        ClearText();
        CameraChange(true);
        ShowUI(true, .25f, .025f);
    }

    public void MyMethod()
    {
        if (!currentlyInDialogue)
        {
            if (canExit)
            {
                CameraChange(false);
                ShowUI(false, .2f, 0);
                Sequence s = DOTween.Sequence();
                s.AppendInterval(.8f);
                s.AppendCallback(() => ResetState());
            }

            if (nextDialogue)
            {
                dialogueText.ReadText(currentInteractible.dialogue.sentences[dialogueIndex]);
            }
        }
        else
        {
            StartDialogue();
        }
    }

    public void ShowUI(bool show, float time, float delay)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(delay);
        sequence.Append(canvasGroup.DOFade(show ? 1 : 0, time));

        if (show)
        {
            dialogueIndex = 0;
            sequence.Join(canvasGroup.transform.DOScale(0, time * 2).From().SetEase(Ease.OutBack));
            sequence.AppendCallback(() => dialogueText.ReadText(currentInteractible.dialogue.sentences[0]));
        }
    }

    public void ClearText()
    {
        dialogueText.text = string.Empty;
    }

    public void ResetState()
    {
        //currentInteractible.Reset(); //reset the animator of currentInteractible
        PlayerManager.instance.ResetAfterDialogue();
        currentlyInDialogue = false;
        canExit = false;
    }

    public void FinishDialogue()
    {
        if (dialogueIndex < currentInteractible.dialogue.sentences.Count - 1)
        {
            dialogueIndex++;
            nextDialogue = true;
        }
        else
        {
            nextDialogue = false;
            canExit = true;
        }
    }

    //The character's name:
    // public void SetNameTextAndColor()
    // {
    //     speakerNameText.text = currentInteractible.data.villagerName;
    //     speakerNameText.color = currentInteractible.data.villagerNameColor;
    //     nameBubble.color = currentInteractible.data.villagerColor;

    // }

    public void CameraChange(bool dialogue)
    {
        gameCam.SetActive(!dialogue);
        dialogueCam.SetActive(dialogue);

        //Depth of field modifier
        if (dialogueDof != null)
        {
            float dofWeight = dialogueCam.activeSelf ? 1 : 0;
            DOVirtual.Float(dialogueDof.weight, dofWeight, .8f, DialogueDOF);
        }
    }

    public void DialogueDOF(float x)
    {
        dialogueDof.weight = x;
    }
}
