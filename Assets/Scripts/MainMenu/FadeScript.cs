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
        [SerializeField] CanvasGroup pressAnyKeyCanvasGroup;
        [Header("Buttons Canvas Group")]
        [SerializeField] public CanvasGroup buttonsGroup;
        [SerializeField] float fadeDelay = 3;
        float fadeTime;

        private void OnEnable()
        {
            if (buttonsGroup == null)
            {
                if (GameObject.Find("ButtonsCanvas__________").TryGetComponent<CanvasGroup>(out CanvasGroup theCanvasGroup))
                {
                    buttonsGroup = theCanvasGroup;
                }
            }

            if (buttonsGroup != null)
            {
                buttonsGroup.alpha = 0;
            }
        }

        public void FadeInInspector()
        {
            fadeTime = Time.time + fadeDelay;
            buttonsGroup.alpha = 0;
            StartCoroutine(myCoroutine());
        }

        private IEnumerator myCoroutine()
        {
            while (Time.time < fadeTime || buttonsGroup.alpha < 1)
            {
                buttonsGroup.alpha += Time.deltaTime;
                yield return null;
            }
        }
    }
}
