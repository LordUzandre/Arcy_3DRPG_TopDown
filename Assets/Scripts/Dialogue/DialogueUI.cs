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
        /// This script should (only) handle the visuals for DialogueUI;
        /// DialogueManager => dialogue.db => DialogueUI
        /// </summary>

        //[SerializeField] public TMP_Animated dialogueText;
        [SerializeField] private Image _dialogueBg;
        //[SerializeField] private Image _dialogueArrow;

        [SerializeField] public List<DialogueAnswerBtn> answrBtns;
        [SerializeField] private GameObject _nextBtn;
        private CanvasGroup _cvGroup;

        private void OnEnable()
        {
            CheckComponents();
            _cvGroup.alpha = 0;
        }

        private void CheckComponents()
        {
            _dialogueBg ??= GetComponentInChildren<Image>();
            _cvGroup ??= TryGetComponent<CanvasGroup>(out CanvasGroup hit) ? hit : null;
        }

        // When we reach the end of a lien of dialogue, should we activate _nextBtn pr answerBtns?
        public void EnableDialogueBtns(bool enableAnswrBtns, bool hideAll = false)
        {
            if (hideAll) // Hide all btns when a new line of dialogue is printed.
            {
                foreach (DialogueAnswerBtn btn in answrBtns)
                {
                    btn.gameObject.SetActive(false);
                    btn.btn.interactable = false;
                }

                _nextBtn?.gameObject.SetActive(false);

                return;
            }

            if (enableAnswrBtns) // Is the talker asking a question?
            {
                _nextBtn?.gameObject.SetActive(false);

                foreach (DialogueAnswerBtn btn in answrBtns)
                {
                    btn.gameObject.SetActive(true);
                    btn.btn.interactable = true;
                }

                // Remember: Check what type of input we are using!
                answrBtns[0].btn.Select();
            }
            else // if not, just continue
            {
                _nextBtn?.gameObject.SetActive(true);
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
                _cvGroup.DOFade(show ? 1 : 0, time);
                yield return routineDelay;

                if (show)
                {
                    DialogueManager.Instance.dialogueIndex = 0;

                    //pop the size of the UI
                    _cvGroup.transform.DOScale(0, time).From().SetEase(Ease.OutBack);
                    yield return routineDelay;
                }
            }
        }
    }
}