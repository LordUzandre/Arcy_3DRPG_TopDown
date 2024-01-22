using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.MainMenu
{
    public class FadeScript : MonoBehaviour
    {
        [Header("PressAnyKey Canvas Group")]
        [SerializeField] private CanvasGroup _pressAnyKeyCanvasGroup;
        [Header("Buttons Canvas Group")]
        [SerializeField] public CanvasGroup buttonsGroup;
        [SerializeField] private float _fadeDelay = 3;
        private float _fadeTime;

        private void OnEnable()
        {
            if (buttonsGroup == null)
                if (GameObject.Find("ButtonsCanvas__________").TryGetComponent<CanvasGroup>(out CanvasGroup theCanvasGroup))
                    buttonsGroup = theCanvasGroup;

            if (buttonsGroup != null)
                buttonsGroup.alpha = 0;
        }

        public void FadeInInspector()
        {
            _fadeTime = Time.time + _fadeDelay;
            buttonsGroup.alpha = 0;
            StartCoroutine(myCoroutine());
        }

        private IEnumerator myCoroutine()
        {
            while (Time.time < _fadeTime || buttonsGroup.alpha < 1)
            {
                buttonsGroup.alpha += Time.deltaTime;
                yield return null;
            }
        }
    }
}
