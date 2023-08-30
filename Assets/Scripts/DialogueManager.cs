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

    [Header("Bools")]
    public bool currentlyInDialogue = false;
    public bool nextDialogue = false;
    public bool canExit = false;
    private int dialogueIndex = 0;
    [Space]
    [Header("Dialogue Canvas Group")]
    public CanvasGroup canvasGroup;
    //    public TMP_Animated animatedText;
    [Space]
    [Header("UI")]
    public DialogueUI dialogueUI;
    public Image dialogueBox;
    [Space]
    [Header("Cameras")]
    public GameObject gameCam;
    public GameObject dialogueCam;
    [SerializeField] public CinemachineTargetGroup targetGroup;
    [Space]
    [Header("Post-proccessing")]
    public UnityEngine.Rendering.Volume dialogueDof;

    [HideInInspector] public TextMeshProUGUI speakerNameText;
    [HideInInspector] public TextMeshProUGUI dialogueText;
    [HideInInspector] public Interactible currentInteractible;
    public TMP_Animated animatedText;

    private void Awake()
    {
        if (instance == null) { instance = this; } else { Destroy(this); }
    }

    private void Start()
    {
        //Set Up UI
        if (dialogueUI == null)
        {
            dialogueUI = GameObject.FindGameObjectWithTag("DialogueUI").GetComponent<DialogueUI>();
        }

        if (dialogueUI != null)
        {
            dialogueBox = dialogueUI.dialogueBox;
            dialogueText = dialogueUI.dialogueText;
            dialogueUI.gameObject.SetActive(false);
        }
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

        // UI
        //ui.SetNameTextAndColor(); //FIX LATER
        currentlyInDialogue = true;
        CameraChange(true);
        ClearText();
        ShowUI(true, .2f, .65f);
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
            sequence.AppendCallback(() => animatedText.ReadText(currentInteractible.dialogue.sentences[0]));
        }
    }

    public void ClearText()
    {
        dialogueText.text = string.Empty;
    }

    public void ResetState()
    {
        //currentInteractible.Reset(); //reset currentInteractible animator
        PlayerManager.instance.ResetAfterDialogue();
        currentlyInDialogue = false;
        canExit = false;
    }

    public void FinishDialogue()
    {
        int sentenceCount = currentInteractible.dialogue.sentences.Count;

        if (dialogueIndex < sentenceCount - 1)
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
