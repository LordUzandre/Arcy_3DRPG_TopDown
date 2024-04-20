using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Player
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        [HideInInspector] public PlayerManager playerManager;
        // [HideInInspector] public float locomotion;
        [SerializeField] public Transform playerEyeLevel;
        private string locomotionBlend = "LocomotionBlend";

        private void Start()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        public void UpdateLocomotion(float moveAmount)
        {
            playerManager.animator.SetFloat(locomotionBlend, moveAmount, .2f, Time.deltaTime);
        }

        public void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
        {
            playerManager.applyRootMotion = applyRootMotion;
            playerManager.animator.CrossFade(targetAnimation, 0.2f);

            //  CAN BE USED TO STOP CHARACTER FROM ATTEMPTING NEW ACTIONS
            //  FOR EXAMPLE, IF YOU GET DAMAGED, AND BEGIN PERFORMING A DAMAGE ANIMATION
            //  THIS FLAG WILL TURN TRUE IF YOU ARE STUNNED
            //  WE CAN THEN CHECK FOR THIS BEFORE ATTEMPTING NEW ACTIONS
            playerManager.isPerformingActionFlag = isPerformingAction;
            playerManager.canRotate = canRotate;
            playerManager.canMove = canMove;
        }
    }
}
