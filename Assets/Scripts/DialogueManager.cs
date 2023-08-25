using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //singleton
    private static DialogueManager instance;
    private static bool isCurrentlyTyping = false;

    [HideInInspector] public TextMeshProUGUI nameText;
    [HideInInspector] public TextMeshProUGUI dialogueText;

    private Queue<string> sentences;

    private void Awake()
    {
        if (instance == null) { instance = this; } else { Destroy(this); }
    }

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //OPEN UI

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
        //Remove UI and change UI;
    }
}
