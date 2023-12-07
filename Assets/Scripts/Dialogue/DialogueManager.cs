//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Cinemachine;
using Arcy.Interaction;

namespace Arcy.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        //singleton
        public static DialogueManager instance;

        //public:
        [Header("Dialogue Canvas Group")]
        [SerializeField]
        public CanvasGroup cvGroup;
        [Space]
        [Header("Cameras")]
        [SerializeField]
        public GameObject gameCam;
        [SerializeField]
        public GameObject dialogueCam;
        [SerializeField]
        public CinemachineTargetGroup targetGroup;
        [Space]
        [Header("Post-proccessing")]
        [SerializeField]
        public UnityEngine.Rendering.Volume dialogueDof;
        [Space]
        [Header("Dialogue Text")]
        [SerializeField]
        public TMP_Animated dialogueText;

        [HideInInspector]
        public SpeakingBase currentInteractible;

        //private:
        private bool _currentlyInDialogue = false;
        private bool _nextDialogue = false;
        private bool _canExit = false;
        private int _dialogueIndex = 0;

        private void Awake()
        {
            if (instance == null) { instance = this; } else { Destroy(this); }
        }

        private void Start()
        {
            if (cvGroup == null)
            {
                GameObject.FindGameObjectWithTag("DialogueUI").GetComponent<CanvasGroup>();
            }

            if (gameCam == null)
            {
                GameObject.FindGameObjectWithTag("MainCamera");
            }

            cvGroup.alpha = 0;
            dialogueText.onDialogueFinish.AddListener(() => FinishDialogue());
        }

        public void RunDialogue(SpeakingBase currentInteractible)
        {
            if (!_currentlyInDialogue)
            {
                StartDialogue();
            }
            else
            {
                if (_canExit)
                {
                    CameraChange(false);
                    FadeUI(false, .2f, .05f);
                    Sequence sequence = DOTween.Sequence();
                    sequence.AppendInterval(.8f);
                    sequence.AppendCallback(() => ResetState());
                }

                if (_nextDialogue)
                {
                    dialogueText.ReadText(currentInteractible.dialogue.Sentences[_dialogueIndex]);
                }
            }
        }

        public void StartDialogue()
        {
            currentInteractible = PlayerManager.instance.currentInteractible as SpeakingBase;

            //camera settings
            targetGroup.m_Targets[1].target = currentInteractible.gameObject.transform;
            _currentlyInDialogue = true;
            ClearText();
            CameraChange(true);
            FadeUI(true, .25f, .025f);
        }

        public void FadeUI(bool show, float time, float delay)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(delay);
            sequence.Append(cvGroup.DOFade(show ? 1 : 0, time));

            if (show)
            {
                _dialogueIndex = 0;
                sequence.Join(cvGroup.transform.DOScale(0, time * 2).From().SetEase(Ease.OutBack));
                sequence.AppendCallback(() => dialogueText.ReadText(currentInteractible.dialogue.Sentences[0]));
            }
        }

        public void ClearText()
        {
            dialogueText.text = string.Empty;
        }

        public void ResetState()
        {
            PlayerManager.instance.DisableMovement(true);
            _currentlyInDialogue = false;
            _canExit = false;
        }

        public void FinishDialogue()
        {
            if (_dialogueIndex < currentInteractible.dialogue.Sentences.Count - 1)
            {
                _dialogueIndex++;
                _nextDialogue = true;
            }
            else
            {
                _nextDialogue = false;
                _canExit = true;
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