using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Arcy.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        //Public:
        [Header("Canvas Groups")]
        [SerializeField] CanvasGroupFader[] cvGroup;
        [Space]
        [SerializeField] GameObject continueButton;

        [Header("Event System")]
        [SerializeField] EventSystem eventSystem;
        [SerializeField] InputSystemUIInputModule inputUIModule;
        [Space]
        [Header("Debug")]
        [Space]
        [SerializeField] bool startGameInPressAnyKeyMode;

        public static Action FadeOut;

        //Private:
        bool initialBool = true;
        float fadeDelay = 2f;
        float fadeTime;
        float fadeNormalized;
        private void OnEnable()
        {
            if (eventSystem == null)
            {
                eventSystem = GameObject.Find("EventSystem")?.GetComponent<EventSystem>();
            }

            if (inputUIModule == null)
            {
                inputUIModule = eventSystem.gameObject.GetComponent<InputSystemUIInputModule>();
            }

            if (cvGroup.Length == 0)
            {
                cvGroup = GameObject.FindObjectsOfType<CanvasGroupFader>();
            }

            inputUIModule.enabled = false;
            eventSystem.SetSelectedGameObject(null);
            fadeNormalized = 1 / fadeDelay;

            if (!startGameInPressAnyKeyMode)
            {
                PressAnyKey_Pressed();
            }
        }

        private void Update()
        {
            if (initialBool)
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    initialBool = false;
                    PressAnyKey_Pressed();
                }
            }
        }

        //Activates when pressing ("Press any key Button"), or if the game shouldn't start in "Press any Key"-mode
        public void PressAnyKey_Pressed()
        {
            foreach (CanvasGroupFader cvFader in cvGroup)
            {
                cvFader.FadeCanvasGroup(false);
            }

            inputUIModule.enabled = true;
            eventSystem.SetSelectedGameObject(continueButton);
        }
    }
}
