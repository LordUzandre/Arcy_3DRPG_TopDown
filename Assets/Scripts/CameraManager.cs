using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Vector3 = UnityEngine.Vector3;

namespace Arcy.Camera
{
    public class CameraManager : MonoBehaviour
    {
        //public:
        [Header("CineMachine Brain")]
        public CinemachineBrain CM_Brain;

        [Header("Main Camera")]
        public CinemachineVirtualCamera gameplayCamera;
        [Space]
        [Header("Dialogue Camera")]
        public CinemachineVirtualCamera dialogueCamera;
        [Space]
        [Header("Top View Camera")]
        public CinemachineVirtualCamera topViewCamera;

        [HideInInspector] public static CameraManager instance;
        [HideInInspector] public float distanceToPlayer;

        CinemachineCollider cinemachineCollider;

        bool playerIsBlockedBool;
        // public delegate void BoolDelegate(bool value);
        // public static event BoolDelegate OnFooBoolUpdated;
        public static Action<bool> OnFooBoolUpdated;

        //Private:
        private Transform player;
        private Vector3 camBodyPosOffset;

        private void Reset()
        {
            CheckComponents();
        }

        private void OnEnable()
        {
            CheckComponents();

            camBodyPosOffset = gameplayCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
            //StartCoroutine(MyCoroutine());

            OnFooBoolUpdated += HandlePlayerBlocked;
        }

        void OnDisable()
        {
            // Unsubscribe from the event to avoid memory leaks
            OnFooBoolUpdated -= HandlePlayerBlocked;
        }

        void HandlePlayerBlocked(bool newValue)
        {
            playerIsBlockedBool = newValue;
            print("We got it this far, didn't we");
        }

        // Call this method to manually update fooBool
        public void UpdateFooBool()
        {
            if (gameplayCamera != null && cinemachineCollider != null)
            {
                bool newValue = cinemachineCollider.IsTargetObscured(gameplayCamera);
                // Trigger the event to notify other components about the change
                OnFooBoolUpdated?.Invoke(newValue);
            }
        }

        private void CheckComponents()
        {
            if (gameplayCamera == null)
            {
                gameplayCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
            }

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            distanceToPlayer = Vector3.Distance((gameplayCamera.transform.position + camBodyPosOffset), player.position) + 2f;
        }

        IEnumerator MyCoroutine()
        {
            yield return new WaitForSeconds(1f);

            while (true)
            {
                bool myBool = gameplayCamera.GetComponent<CinemachineCollider>().IsTargetObscured(gameplayCamera);
                //print(myBool);
                yield return new WaitForSeconds(.5f);
            }
        }
    }
}