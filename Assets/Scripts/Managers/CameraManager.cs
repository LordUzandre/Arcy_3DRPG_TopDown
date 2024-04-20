using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using DG.Tweening;
using Arcy.Dialogue;
using Arcy.Interaction;
using Arcy.Management;

namespace Arcy.Camera
{
    public class CameraManager : MonoBehaviour
    {
        #region Serializefield components

        //public:
        [Header("CineMachine Brain")]
        public CinemachineBrain CM_Brain;

        [Header("cameras")]
        [SerializeField] public CinemachineVirtualCamera gameplayCamera;
        [SerializeField] public CinemachineVirtualCamera topViewCamera;
        [SerializeField] public CinemachineVirtualCamera dialogueCamera;

        [Header("Dialogue Specific")]
        [SerializeField] public CinemachineTargetGroup targetGroup;
        [SerializeField] public Volume dialogueDof;
        #endregion

        // Singleton
        private static CameraManager instance;
        public static CameraManager Instance { get; private set; }

        // private List<CinemachineVirtualCamera> cameraList = new List<CinemachineVirtualCamera>();
        [SerializeField] private Dictionary<int, CinemachineVirtualCamera> _cameraList = new Dictionary<int, CinemachineVirtualCamera>();
        private float _distanceToPlayer;
        private int _camListIndex = 0;
        private bool _isFreeroam;

        #region Subscriptions

        private void Start()
        {
            _cameraList.Clear();
            _cameraList.Add(0, gameplayCamera);
            _cameraList.Add(1, topViewCamera);
            _cameraList.Add(2, dialogueCamera);
        }

        private void OnEnable()
        {
            GameManager.instance.gameStateManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.instance.gameStateManager.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newGameState)
        {
            StopCoroutine(BlockedViewChecker());

            switch (newGameState)
            {
                case GameState.Freeroam:
                    _isFreeroam = true;
                    ChangeCamera(0);
                    StartCoroutine(BlockedViewChecker());
                    break;
                case GameState.Dialogue:
                    _isFreeroam = false;
                    // Don't change camera if the ISpeakable is a sign
                    if (Player.PlayerManager.instance.currentInteractible is Sign)
                        break;

                    ChangeCamera(2);
                    break;
                default:
                    break;
            }

            StartCoroutine(CheckTargetGroup());
        }

        #endregion

        #region Freeroam
        // Coroutine that checks whether Freeroam-Camera is blocked
        private IEnumerator BlockedViewChecker()
        {
            bool targetIsObscured;
            WaitForSeconds delay = new WaitForSeconds(.4f);

            yield return delay;

            if (GameManager.instance.gameStateManager.CurrentGameState == GameState.Freeroam)
            {
                _isFreeroam = true;
            }
            else
            {
                yield break;
            }

            while (_isFreeroam == true)
            {
                //Check whether player is obscured
                targetIsObscured = gameplayCamera.GetComponent<CinemachineCollider>().IsTargetObscured(gameplayCamera);

                //Switch to topview camera
                ChangeCamera(targetIsObscured ? 1 : 0);

                yield return delay;
            }
        }
        #endregion

        void ChangeCamera(int camIndex)
        {
            // Check so that we actually need to change camera
            if (camIndex == _camListIndex)
                return;

            _camListIndex = camIndex;

            foreach (KeyValuePair<int, CinemachineVirtualCamera> camera in _cameraList)
            {
                if (camera.Key == camIndex)
                {
                    _cameraList[camera.Key].Priority = 10;
                }
                else
                {
                    _cameraList[camera.Key].Priority = 1;
                }
            }

            // // Depth of field modifier
            // if (dialogueDof != null)
            // {
            //     float dofWeight = dialogueCamera.enabled ? 1 : 0;
            //     DOVirtual.Float(dialogueDof.weight, dofWeight, .8f, DialogueDOF);
            // }
        }

        IEnumerator CheckTargetGroup()
        {
            yield return null;

            if (GameManager.instance.gameStateManager.CurrentGameState == GameState.Dialogue)
            {
                targetGroup.m_Targets[1].target = GameManager.instance.dialogueManager.otherSpeakerTransform;
            }
            else
            {
                targetGroup.m_Targets[1].target = null;
            }
        }

        // public void DialogueDOF(float x)
        // {
        //     dialogueDof.weight = x;
        // }
    }
}