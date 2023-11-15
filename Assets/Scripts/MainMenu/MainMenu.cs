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
        [SerializeField] CanvasGroup pressAnyKey_CanvasGroup;
        [SerializeField] CanvasGroup buttons_Group;
        [SerializeField] CanvasGroup blackOverlay;
        [Space]
        [SerializeField] GameObject continueButton;

        [Header("Event System")]
        [SerializeField] EventSystem eventSystem;
        [SerializeField] InputSystemUIInputModule inputUIModule;

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

            if (buttons_Group == null)
            {
                buttons_Group = GameObject.Find("Canvas__Buttons")?.GetComponent<CanvasGroup>();
            }

            if (buttons_Group != null)
            {
                if (pressAnyKey_CanvasGroup.gameObject.activeInHierarchy)
                {
                    buttons_Group.alpha = .05f;
                    inputUIModule.enabled = false;
                    eventSystem.SetSelectedGameObject(null);
                }
            }

            if (blackOverlay != null)
            {
                blackOverlay.alpha = 1f;
            }

            fadeNormalized = 1 / fadeDelay;
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

        public void ContinueButtonPressed()
        {
            fadeTime = Time.time + fadeDelay;
            StartCoroutine(ChangeScene());

            IEnumerator ChangeScene()
            {
                while (Time.time < fadeTime)
                {
                    blackOverlay.alpha += (Time.deltaTime * fadeNormalized);

                    if (blackOverlay.alpha > (1 - Time.deltaTime))
                    {
                        print("Scene Loaded");
                        StopCoroutine(ChangeScene());
                        //SceneManager.LoadScene("DebugScene");
                    }

                    yield return null;
                }
            }
        }

        public void NewGameButton()
        {
            print("New Game Button Pressed");
        }

        public void OptionsButtonPressed()
        {
            print("Options Button Pressed");
        }

        public void QuitButtonPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        //Activates when pressing ("Press any key Button")
        public void PressAnyKey_Pressed()
        {
            fadeTime = Time.time + fadeDelay;
            buttons_Group.alpha = 0;
            inputUIModule.enabled = true;
            eventSystem.SetSelectedGameObject(continueButton);
            StartCoroutine(FadeInspector());

            IEnumerator FadeInspector()
            {
                while (fadeTime - Time.time > 1 || buttons_Group.alpha < 1)
                {
                    buttons_Group.alpha += (Time.deltaTime * (fadeNormalized * 2));
                    pressAnyKey_CanvasGroup.alpha -= (Time.deltaTime * (fadeNormalized * 10));
                    blackOverlay.alpha -= (Time.deltaTime * fadeNormalized);
                    yield return null;
                }

                if (buttons_Group.alpha >= 1)
                {
                    Destroy(pressAnyKey_CanvasGroup.gameObject);
                    blackOverlay.alpha = 0;
                }
            }
        }
    }
}
