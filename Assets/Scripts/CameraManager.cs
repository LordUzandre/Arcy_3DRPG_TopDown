using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using DG.Tweening;

namespace Arcy.Camera
{
    public class CameraManager : MonoBehaviour
    {
        #region Serializefield components
        //public:
        [Header("CineMachine Brain")]
        public CinemachineBrain CM_Brain;
        [Space]
        [Header("cameras")]
        public CinemachineVirtualCamera gameplayCamera;
        public CinemachineVirtualCamera topViewCamera;
        public CinemachineVirtualCamera dialogueCamera;
        [Header("Dialogue Specific")]
        public CinemachineTargetGroup targetGroup;
        [SerializeField] public Volume dialogueDof;
        #endregion

        private static CameraManager instance;
        public static CameraManager Instance { get; private set; }

        private List<CinemachineVirtualCamera> cameraList = new List<CinemachineVirtualCamera>();
        [HideInInspector] public float distanceToPlayer;
        [HideInInspector] public int cameraListIndex;
        private bool _isFreeroam;

        #region Subscriptions
        private void OnEnable()
        {
            GameStateChanged(GameStateManager.Instance.CurrentGameState);
            GameStateManager.OnGameStateChanged += GameStateChanged;

            cameraList.Clear();
            cameraList.Add(gameplayCamera);
            cameraList.Add(topViewCamera);
            cameraList.Add(dialogueCamera);

            //myMethod += gameplayCamera.GetComponent<CinemachineCollider>().IsTargetObscured(gameplayCamera);
        }

        private void OnDisable()
        {
            GameStateManager.OnGameStateChanged -= GameStateChanged;
        }
        #endregion

        #region Freeroam
        IEnumerator CheckIfCameraIsBlockedInFreeroam()
        {
            yield return new WaitForSeconds(.2f);

            if (GameStateManager.Instance.CurrentGameState != GameState.Freeroam)
            {
                yield return null;
            }
            else
            {
                _isFreeroam = true;
            }

            bool targetIsObscured;
            WaitForSeconds delay = new WaitForSeconds(.4f);

            while (_isFreeroam == true)
            {
                //Check whether player is obscured
                targetIsObscured = gameplayCamera.GetComponent<CinemachineCollider>().IsTargetObscured(gameplayCamera);

                //Switch to topview camera
                ChangeCamera(targetIsObscured ? 1 : 0, true);

                yield return delay;
            }

        }
        #endregion

        void GameStateChanged(GameState newGameState)
        {
            StopCoroutine(CheckIfCameraIsBlockedInFreeroam());

            switch (newGameState)
            {
                case GameState.Freeroam:
                    _isFreeroam = true;
                    ChangeCamera(0, true);
                    StartCoroutine(CheckIfCameraIsBlockedInFreeroam());
                    break;
                case GameState.Dialogue:
                    _isFreeroam = false;
                    ChangeCamera(2, true);
                    break;
                default:
                    break;
            }
        }

        public Transform CheckCurrentCamera()
        {
            return cameraList[cameraListIndex].transform;
        }

        void ChangeCamera(int index = 0, bool cameraChange = false)
        {
            // Check so that we actually need to change camera
            if (index == cameraListIndex)
            {
                //print("CameraManager: index == cameraIndex");
                return;
            }

            cameraListIndex = index;

            foreach (CinemachineVirtualCamera camera in cameraList)
            {
                camera.Priority = 9;
            }

            cameraList[index].Priority = 10;

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