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

namespace Arcy.Dialogue
{
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
        [Space]

        [Header("Dialogue Text")]
        public TMP_Animated dialogueText;

        [HideInInspector] public Interactible currentInteractible;
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

            if (gameCam == null)
            {
                GameObject.FindGameObjectWithTag("MainCamera");
            }

            canvasGroup.alpha = 0;
            dialogueText.onDialogueFinish.AddListener(() => FinishDialogue());
        }

        public void RunDialogue(Interactible currentInteractible)
        {
            if (!currentlyInDialogue)
            {
                StartDialogue();
            }
            else
            {
                if (canExit)
                {
                    CameraChange(false);
                    FadeUI(false, .2f, .05f);
                    Sequence sequence = DOTween.Sequence();
                    sequence.AppendInterval(.8f);
                    sequence.AppendCallback(() => ResetState());
                }

                if (nextDialogue)
                {
                    dialogueText.ReadText(currentInteractible.dialogue.Sentences[dialogueIndex]);
                }
            }
        }

        public void StartDialogue()
        {
            currentInteractible = PlayerManager.instance.currentInteractible;

            if (currentInteractible.TryGetComponent<NPC_AnimationHandler>(out NPC_AnimationHandler npcAnimator))
            {
                npcAnimator.TurnToPlayer(transform.position);
            }

            //camera settings
            targetGroup.m_Targets[1].target = currentInteractible.gameObject.transform;
            currentlyInDialogue = true;
            ClearText();
            CameraChange(true);
            FadeUI(true, .25f, .025f);
        }

        public void FadeUI(bool show, float time, float delay)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(delay);
            sequence.Append(canvasGroup.DOFade(show ? 1 : 0, time));

            if (show)
            {
                dialogueIndex = 0;
                sequence.Join(canvasGroup.transform.DOScale(0, time * 2).From().SetEase(Ease.OutBack));
                sequence.AppendCallback(() => dialogueText.ReadText(currentInteractible.dialogue.Sentences[0]));
            }
        }

        public void ClearText()
        {
            dialogueText.text = string.Empty;
        }

        public void ResetState()
        {
            PlayerManager.instance.ResetAfterDialogue();
            currentlyInDialogue = false;
            canExit = false;
        }

        public void FinishDialogue()
        {
            if (dialogueIndex < currentInteractible.dialogue.Sentences.Count - 1)
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

        public void CameraChange(bool dialogue) //true = dialogue, false = freeroam
        {
            if (dialogueCam != null)
            {
                gameCam.SetActive(!dialogue);
                dialogueCam.SetActive(dialogue);
            }

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

}