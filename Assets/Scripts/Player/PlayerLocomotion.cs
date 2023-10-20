using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.InputManager;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;

    public float moveAmount;

    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float rotationSpeed = 15;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private Vector3 GetYourBearings()
    {
        Vector3 worldSetting;

        // Create switch statement based on either world nornal or camera
        worldSetting = (InputManager.instance.inputY * Vector3.forward) + (InputManager.instance.inputX * Vector3.right);
        worldSetting.Normalize();
        worldSetting.y = 0;

        return worldSetting;
    }

    public void HandleAllMovement(float delta)
    {
        Vector3 moveDirection = GetYourBearings();

        HandleGroundedMovement(delta, moveDirection);
        HandleRotation(delta, moveDirection);
    }

    public void HandleGroundedMovement(float delta, Vector3 moveDirection)
    {
        if (InputManager.instance.moveAmount > 0.5f)
        {
            playerManager.characterController.Move(moveDirection * runningSpeed * delta);
        }
        else if (InputManager.instance.moveAmount <= 0.5f)
        {
            playerManager.characterController.Move(moveDirection * walkingSpeed * delta);
        }

        playerManager.animationHandler.UpdateLocomotion();
    }

    private void HandleRotation(float delta, Vector3 targetRotationDirection)
    {
        if (!playerManager.canRotate)
            return;

        //prevent the character to rotate back to zero
        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
            return;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * delta);
        transform.rotation = targetRotation;
    }

    public void AttemptToPerformDodge()
    {
        Vector3 rollDirection;

        if (playerManager.isPerformingAction)
            return;

        if (InputManager.instance.moveAmount > 0) //roll if we are moving
        {
            rollDirection = GetYourBearings();

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            playerManager.transform.rotation = playerRotation;

            playerManager.animationHandler.PlayTargetActionAnimation("Roll_Forward_01", true, true);
        }
        else //Backstep
        {
            playerManager.animationHandler.PlayTargetActionAnimation("Back_Step_01", true, true);
        }
    }
}
