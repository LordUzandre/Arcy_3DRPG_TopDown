//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = System.Random;

public class DialogueManager : MonoBehaviour
{
    //singleton
    private static DialogueManager instance;
    public static DialogueManager Instance { get { return instance; } }
    private static bool isCurrentlyTyping = false;
    public DialogueUI dialogueUI;
    public Image dialogueBox;

    [HideInInspector] public TextMeshProUGUI nameText;
    [HideInInspector] public TextMeshProUGUI dialogueText;

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
            sentences = new Queue<string>();
            dialogueBox = dialogueUI.dialogueBox;
            dialogueText = dialogueUI.dialogueText;
            nameText = dialogueUI.nameText;
            dialogueUI.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Could not find dialogueUI");
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //OPEN UI
        dialogueUI.gameObject.SetActive(true);
        nameText.text = dialogue.nameOfSpeaker;
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
