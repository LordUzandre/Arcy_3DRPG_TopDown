using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.MainMenu
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupFader : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] public float fadeTime = 1;

        private void OnEnable()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        public void FadeCanvasGroup(bool fadeToBlack)
        {
            StartCoroutine(myCoroutine(fadeToBlack));
        }

        private IEnumerator myCoroutine(bool fadeToBlack)
        {
            float myTimer = Time.time + fadeTime;
            float fadeTimeNormalized = 1 / fadeTime;

            if (fadeToBlack)
            {
                while (Time.time < myTimer)
                {
                    canvasGroup.alpha += (Time.deltaTime * fadeTimeNormalized);
                    yield return null;
                }
            }
            else if (!fadeToBlack)
            {
                while (Time.time < myTimer)
                {
                    canvasGroup.alpha -= (Time.deltaTime * fadeTimeNormalized);
                    yield return null;
                }
            }
        }
    }
}
