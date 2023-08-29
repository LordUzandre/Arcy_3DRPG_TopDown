//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Random = System.Random;

public class DialogueManager : MonoBehaviour
{
    //singleton
    public static DialogueManager instance;

    //bools
    public bool currentlyInDialogue = false;
    public bool nextDialogue = false;
    public bool canExit = false;
    public bool isCurrentlyTyping = false;
    //int
    public int dialogueIndex = 0;

    public CanvasGroup canvasGroup;
    //    public TMP_Animated animatedText;
    public DialogueUI dialogueUI;
    public Image dialogueBox;

    [HideInInspector] public TextMeshProUGUI dialogueText;
    [HideInInspector] public Interactible currentInteractible;

    private Queue<string> sentences = new Queue<string>();

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
        else
        {
            Debug.LogWarning("Could not find dialogueUI");
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
            //sequence.AppendCallback(() => animatedText.ReadText(currentVillager.dialogue.conversationBlock[0]));
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //OPEN UI
        dialogueUI.gameObject.SetActive(true);
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();

        if (!isCurrentlyTyping)
        {
            //Start typing out sentence
            StartCoroutine(TypeSentence(sentence));
        }
        else
        {
            //Display full sentence
            dialogueText.text = sentence;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isCurrentlyTyping = true;
        dialogueText.text = " ";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    private void EndDialogue()
    {
        //Remove UI and change playerManager;
        dialogueBox.gameObject.SetActive(false);
        PlayerManager.instance.isInteracting = false;
    }
}
