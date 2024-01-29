using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

namespace Arcy.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {

        /// <summary>
        /// This script SHOULD only handle the visuals for DialogueUI (right now it does more);
        /// DialogueManager => dialogue.db => DialogueUI
        /// </summary>

        [SerializeField] private Image _dialogueBg;
        [SerializeField] public List<DialogueAnswerBtn> answrBtns;
        [SerializeField] private GameObject _nextBtn;
        [HideInInspector] public CanvasGroup cvGroup;

#if UNITY_EDITOR
        private void OnValidate()
        {
            CheckComponents();
        }
#endif

        private void OnEnable()
        {
            CheckComponents();
            cvGroup.alpha = 0;
        }

        private void CheckComponents()
        {
            _dialogueBg ??= GetComponentInChildren<Image>();
            cvGroup ??= TryGetComponent<CanvasGroup>(out CanvasGroup hit) ? hit : null;
        }

        // When we reach the end of a line of dialogue, should we activate _nextBtn or answerBtns?
        public void EnableDialogueBtns(bool enableAnswrBtns, bool hideAll = false)
        {
            if (hideAll) // Hide all btns when a new line of dialogue is printed.
            {
                foreach (DialogueAnswerBtn button in answrBtns)
                {
                    button.gameObject.SetActive(false);
                    button.btn.interactable = false;
                }

                _nextBtn?.gameObject.SetActive(false);

                return;
            }

            if (enableAnswrBtns) // Is the talker asking a question?
            {
                _nextBtn?.gameObject.SetActive(false);

                foreach (DialogueAnswerBtn button in answrBtns)
                {
                    button.gameObject.SetActive(true);
                    button.btn.interactable = true;
                }

                // Remember: Check what type of input we are using!
                answrBtns[0].btn.Select();
            }
            else // if not, just continue and show nextBtn
            {
                _nextBtn?.gameObject.SetActive(true);

                if (!answrBtns[0].btn.interactable)
                    return;

                foreach (DialogueAnswerBtn btn in answrBtns)
                {
                    btn.btn.interactable = false;
                    btn.gameObject.SetActive(false);
                }
            }
        }

        // Fade DialogueUI in or out
        public void FadeDialogueUI(bool show, float time, float delay)
        {
            WaitForSeconds routineDelay = new WaitForSeconds(delay);
            StartCoroutine(fadeUI());

            IEnumerator fadeUI()
            {
                yield return routineDelay;
                cvGroup.DOFade(show ? 1 : 0, time);
                yield return routineDelay;

                if (show)
                {
                    //pop the size of the UI
                    cvGroup.transform.DOScale(0, time).From().SetEase(Ease.OutBack);
                    yield return routineDelay;
                }
            }
        }
    }
}