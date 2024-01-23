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
        [SerializeField] private Image _dialogueArrow;

        [SerializeField] private Button _yesBtn;
        [SerializeField] private Button _noBtn;
        private CanvasGroup _cvGroup;

        private void OnEnable()
        {
            CheckComponents();
            _cvGroup.alpha = 0;
        }

        private void CheckComponents()
        {
            if (_dialogueBg == null)
                _dialogueBg = GetComponentInChildren<Image>();

            if (_dialogueArrow == null)
                _dialogueArrow = GetComponentInChildren<Image>();

            _cvGroup ??= TryGetComponent<CanvasGroup>(out CanvasGroup hit) ? hit : null;
        }

        // Fade DialogueUI in or out
        public void FadeUI(bool show, float time, float delay)
        {
            _yesBtn?.gameObject.SetActive(false);
            _noBtn?.gameObject.SetActive(false);
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