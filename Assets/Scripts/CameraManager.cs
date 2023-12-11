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

        private void OnSceneGUI()
        {
            Debug.DrawLine(gameplayCamera.transform.position + camBodyPosOffset, player.transform.position, Color.yellow, Mathf.Infinity);
        }

        private void FixedUpdate()
        {
            RaycastHit hit;

            if (Physics.Linecast((transform.position + camBodyPosOffset), player.transform.position, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.transform == player)
                    {
                        //Debug.Log("RayCast hit player");
                    }
                }
            }
        }
    }
}