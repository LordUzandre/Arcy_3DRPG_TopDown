using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Arcy.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {

        /// <summary>
        /// This script should (only) handle the visuals for DialogueUI;
        /// DialogueManager => dialogue.db => DialogueUI
        /// </summary>

        [SerializeField] public TMP_Animated dialogueText;
        [SerializeField] private Image _dialogueBox;
        [SerializeField] private Image _dialogueArrow;

        [SerializeField] private Button _yesBtn;
        [SerializeField] private Button _noBtn;
        private CanvasGroup _cvGroup;

        //public bool _currentlyTyping;

        private void OnEnable()
        {
            CheckComponents();
            _cvGroup.alpha = 0;
        }

        private void CheckComponents()
        {
            if (dialogueText == null)
                dialogueText = GetComponentInChildren<TMP_Animated>();

            if (_dialogueBox == null)
                _dialogueBox = GetComponentInChildren<Image>();

            if (_dialogueArrow == null)
                _dialogueArrow = GetComponentInChildren<Image>();

            _cvGroup ??= TryGetComponent<CanvasGroup>(out CanvasGroup hit) ? hit : null;
        }

        public void FadeUI(bool show, float time, float delay, bool btns = false)
        {
            _yesBtn?.gameObject.SetActive(btns);
            _noBtn?.gameObject.SetActive(btns);
            WaitForSeconds routineDelay = new WaitForSeconds(delay);

            dialogueText.text = string.Empty;

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
                    TypeOutDialogueText(0);
                }
            }

            // Og Sequence (redundant)
            // Sequence sequence = DOTween.Sequence();
            // sequence.AppendInterval(delay);

            // // Fade in or out depending on bool
            // sequence.Append(_cvGroup.DOFade(show ? 1 : 0, time));

            // if (show)
            // {
            //     DialogueManager.Instance.dialogueIndex = 0;
            //     //pop the size of the UI
            //     sequence.Join(_cvGroup.transform.DOScale(0, time * 2).From().SetEase(Ease.OutBack));

            //     //Start typing the text after the UI is faded in.
            //     sequence.AppendCallback(() => TypeOutDialogueText(0));
            // }
        }

        public void TypeOutDialogueText(int index)
        {
            dialogueText?.ReadText(DialogueManager.Instance.dialogueBlock[index]);
            //_currentlyTyping = true;
        }
    }
}