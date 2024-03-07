using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.InputManagement;
using Arcy.Management;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;

    [SerializeField] private float _walkingSpeed = 2;
    [SerializeField] private float _runningSpeed = 5;
    [SerializeField] private float _rotationSpeed = 15;

    private float _inputX = 0;
    private float _inputY = 0;
    [SerializeField] private float _moveAmount = 0;
    [SerializeField] private float _movementSpeed = 0;

    private float _gravity = 9.8f;
    private Vector3 _velocity;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        GameEventManager.instance.inputEvents.onWASDInput += HandleMovementInput;
    }

    private void OnDisable()
    {
        GameEventManager.instance.inputEvents.onWASDInput -= HandleMovementInput;
    }

    // Decide walking direction
    private Vector3 GetYourBearings()
    {
        Vector3 worldSetting;

        // Right now, we can only walk based on the world normal
        // REMEMBER: set up a system tht can change the walking direction, 
        // based on whichever scenario we are currently in

        worldSetting = (_inputY * Vector3.forward) + (_inputX * Vector3.right);
        worldSetting.Normalize();
        worldSetting.y = 0;

        return worldSetting;
    }

    // Called every Update from PlayerManager
    public void HandleAllMovement(float delta)
    {
        Vector3 moveDirection = GetYourBearings();

        HandleGroundedMovement(delta, moveDirection);
        HandleRotation(delta, moveDirection);
    }

    private void HandleMovementInput(Vector2 movementInput)
    {
        _inputX = movementInput.x;
        _inputY = movementInput.y;

        _moveAmount = Mathf.Clamp01(Mathf.Abs(_inputY) + Mathf.Abs(_inputX));

        if (_moveAmount <= 0.5 && _moveAmount > 0)
        {
            _moveAmount = 0.5f;
        }
        else if (_moveAmount > 0.5 && _moveAmount <= 1)
        {
            _moveAmount = 1;
        }
    }

    private void HandleGroundedMovement(float delta, Vector3 moveDirection)
    {
        if (_moveAmount > 0.5f)
        {
            _movementSpeed = _runningSpeed;
        }
        else if (_moveAmount <= 0.5f)
        {
            _movementSpeed = _walkingSpeed;
        }

        _velocity = moveDirection * _movementSpeed * delta;

        // Apply gravity
        if (playerManager.characterController.isGrounded)
        {
            _velocity.y = 0;
        }
        else
        {
            _velocity.y = -_gravity;
        }

        playerManager.characterController.Move(_velocity);
        playerManager.animationHandler.locomotion = _moveAmount;
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
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, _rotationSpeed * delta);
        transform.rotation = targetRotation;
    }
}
