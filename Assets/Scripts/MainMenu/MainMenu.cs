using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using DG.Tweening;

namespace Arcy.MainMenu
{

    public class MainMenu : MonoBehaviour
    {
        //Public:
        public enum MainMenuState
        {
            PressAnyKey, MainMenu, Options, QuitGame
        }

        //If we want to change which State we start in
        [SerializeField] public MainMenuState startingMenuState;
        private MainMenuState CurrentMenuState { get; set; }
        public static Action OnMenuStateAction;

        [Header("Canvas Groups")]
        [SerializeField] GameObject[] _cvGroup;
        [SerializeField] GameObject[] _mainMenuButtons;
        [SerializeField] GameObject[] _optionsButtons;

        [Header("Debug")]


        //Private:
        private InputSystemUIInputModule _inputUIModule;
        private MainMenuState _previousState;
        private bool _initialBool = true;
        private float _fadeDelay = 2f;
        private float _fadeTime;
        private float _fadeNormalized;

        private void OnEnable()
        {
            _inputUIModule = EventSystem.current.gameObject.GetComponent<InputSystemUIInputModule>();
            _fadeNormalized = 1 / _fadeDelay;
            OnMenuStateChanged(startingMenuState);
        }

        private void Update()
        {
            if (_initialBool)
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    _initialBool = false;
                    PressAnyKey_Pressed();
                }
            }
        }

        public void OnMenuStateChanged(MainMenuState newMainMenuState)
        {
            if (newMainMenuState == CurrentMenuState)
            {
                return;
            }

            _previousState = CurrentMenuState;
            CurrentMenuState = newMainMenuState;

            StartCoroutine(OnMenuStateCorouitne());

            IEnumerator OnMenuStateCorouitne()
            {
                yield return null;

                switch (newMainMenuState)
                {
                    case MainMenuState.PressAnyKey:

                        //This is the default starting state
                        EventSystem.current.SetSelectedGameObject(null);
                        _inputUIModule.enabled = false;
                        break;

                    case MainMenuState.MainMenu:

                        //Whenever we return to the main menu
                        if (_inputUIModule.enabled == false)
                        {
                            _inputUIModule.enabled = true;
                        }

                        EventSystem.current.SetSelectedGameObject(_mainMenuButtons[0]);

                        //In case we skip the "Press any key"-phase
                        if (_initialBool)
                        {
                            _initialBool = false;
                            PressAnyKey_Pressed();
                        }
                        break;

                    case MainMenuState.Options:
                        //Whenever we enter the options menu

                        //Add Code here

                        break;
                    case MainMenuState.QuitGame:
                        // When we press "Quit"
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#endif
                        Application.Quit();
                        break;
                }
            }
        }

        //Activates when pressing ("Press any key Button"), or if the game shouldn't start in "Press any Key"-mode
        public void PressAnyKey_Pressed()
        {
            OnMenuStateChanged(MainMenuState.MainMenu);

            foreach (GameObject cvFader in _cvGroup)
            {
                cvFader.gameObject.GetComponent<CanvasGroupFader>().FadeCanvasGroup(false);

                //Create an array of all Transforms with a cvFader-component
                Transform[] transformsInChildren = cvFader.gameObject.GetComponentsInChildren<Transform>();

                foreach (Transform transform in transformsInChildren)
                {
                    //Do not bounce BlackOverlay
                    if (cvFader.name != "Canvas__BlackOverlay")
                    {
                        transform.DOPunchScale(new Vector2(-0.1f, -0.1f), 0.2f, 1);
                    }
                }
            }
        }
    }
}
