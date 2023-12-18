using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using DG.Tweening;
using Arcy.Dialogue;

namespace Arcy.Camera
{
    public class CameraManager : MonoBehaviour
    {
        //public:
        [Header("CineMachine Brain")]
        public CinemachineBrain CM_Brain;
        [Space]
        [Header("Main Camera")]
        public CinemachineVirtualCamera gameplayCamera;
        [Header("Top View Camera")]
        public CinemachineVirtualCamera topViewCamera;
        [Space]
        [Header("Dialogue Camera")]
        public CinemachineVirtualCamera dialogueCamera;
        public CinemachineTargetGroup targetGroup;
        [Header("Post-proccessing")]
        [SerializeField]
        public Volume dialogueDof;


        private static CameraManager instance;
        public static CameraManager Instance { get; private set; }

        [HideInInspector] public float distanceToPlayer;

        private CinemachineVirtualCamera currentActiveCamera;
        public CinemachineVirtualCamera CurrentActiveCamera { get; private set; }

        private void OnEnable()
        {
            GameStateManager.OnGameStateChanged += GameStateChanged;
        }

        private void OnDisable()
        {
            GameStateManager.OnGameStateChanged -= GameStateChanged;
        }

        IEnumerator CheckIfCameraIsBlockedInFreeroam()
        {
            yield return new WaitForSeconds(.5f);

            if (GameStateManager.Instance.CurrentGameState != GameState.Freeroam)
            {
                yield return null;
            }

            bool targetIsObscured;
            WaitForSeconds delay = new WaitForSeconds(.4f);

            while (true)
            {
                targetIsObscured = gameplayCamera.GetComponent<CinemachineCollider>().IsTargetObscured(gameplayCamera);

                if (targetIsObscured)
                {
                    topViewCamera.MoveToTopOfPrioritySubqueue();
                    print("CameraManager: target Is Obscured");
                }
                else
                {
                    gameplayCamera.MoveToTopOfPrioritySubqueue();
                }

                yield return delay;
            }
        }

        void GameStateChanged(GameState newGameState)
        {
            switch (newGameState)
            {
                case GameState.Freeroam:
                    gameplayCamera.MoveToTopOfPrioritySubqueue();
                    StartCoroutine(CheckIfCameraIsBlockedInFreeroam());
                    break;
                case GameState.Dialogue:
                    //ChangeToDialogueCamera(DialogueManager.Instance.cameraShouldChange);
                    break;
                default:
                    break;
            }
        }

        void ChangeToDialogueCamera(bool cameraChange)
        {
            //Check wether currentInteractible is a sign or a character
            if (!cameraChange)
            {
                return;
            }

            dialogueCamera.MoveToTopOfPrioritySubqueue();
            print("CameraStateManager is succesful");

            //Depth of field modifier
            if (dialogueDof != null)
            {
                float dofWeight = dialogueCamera.enabled ? 1 : 0;
                DOVirtual.Float(dialogueDof.weight, dofWeight, .8f, DialogueDOF);
            }
        }

        public void DialogueDOF(float x)
        {
            dialogueDof.weight = x;
        }
    }
}