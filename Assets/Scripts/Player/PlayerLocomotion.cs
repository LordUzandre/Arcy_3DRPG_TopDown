using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Management;
using DG.Tweening;

namespace Arcy.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerLocomotion : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] public bool startAtCheckpoint;
        [Space]
        [HideInInspector] public bool characterIsMovingSomewhere = false;

        private PlayerManager _playerManager;
        private CharacterController _charCtrl;

        [SerializeField] private float _walkingSpeed = 2;
        [SerializeField] private float _runningSpeed = 5;
        [SerializeField] private float _rotationSpeed = 15;

        private float _inputX = 0;
        private float _inputY = 0;
        [SerializeField] private float _moveAmount = 0;
        [SerializeField] private float _movementSpeed = 0;
        [Space]
        [Header("VFX")]
        [SerializeField] private ParticleSystem walkingParticles;

        private float _gravity = 9.8f;
        // private Vector3 _velocity;

        private void Start()
        {
            _playerManager = GetComponent<PlayerManager>();
            _charCtrl = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            GameManager.instance.gameEventManager.inputEvents.onWASDInput += HandleMovementInput;
        }

        private void OnDisable()
        {
            GameManager.instance.gameEventManager.inputEvents.onWASDInput -= HandleMovementInput;
        }


        // Called every Update from PlayerManager
        public void HandleAllMovement()
        {
            if (!characterIsMovingSomewhere)
            {
                Vector3 moveDirection = GetYourBearings();

                HandleGroundedMovement(moveDirection);
                HandleRotation(moveDirection);
            }
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

        // Input, always results in a value between 0-1
        private void HandleMovementInput(Vector2 movementInput)
        {
            // TODO - remember to add code for sprinting.

            _inputX = movementInput.x;
            _inputY = movementInput.y;

            _moveAmount = Mathf.Clamp01(Mathf.Abs(_inputY) + Mathf.Abs(_inputX));

            if (_moveAmount <= 0.5f && _moveAmount > 0.01f)
            {
                _moveAmount = 0.5f;
            }
            else if (_moveAmount > 0.5f && _moveAmount <= 1f)
            {
                _moveAmount = 1;
            }
        }

        // CharCtrl.Move()
        private void HandleGroundedMovement(Vector3 moveDirection)
        {
            if (_moveAmount > 0.5f)
            {
                _movementSpeed = _runningSpeed;
            }
            else if (_moveAmount <= 0.5f)
            {
                _movementSpeed = _walkingSpeed;
            }

            Vector3 _velocity = moveDirection * _movementSpeed * Time.deltaTime;

            // Apply gravity
            if (_charCtrl.isGrounded)
            {
                _velocity.y = 0;
            }
            else
            {
                _velocity.y = -_gravity;
            }

            _charCtrl.Move(_velocity);
            _playerManager.animationHandler.UpdateLocomotion(_moveAmount);
        }

        private void HandleRotation(Vector3 targetRotationDirection)
        {
            if (!_playerManager.canRotate)
                return;

            //prevent the character to rotate back to zero
            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
                return;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, _rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        public void MoveToSpecifiedPosition(Vector3 newPos)
        {
            characterIsMovingSomewhere = true;
            StartCoroutine(MoveToTarget(newPos));
        }

        IEnumerator MoveToTarget(Vector3 targetPos)
        {
            // Rotate towards 
            Vector3 lookDirection = (targetPos - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            transform.eulerAngles = newRotation.eulerAngles;

            float charCtrlVelocity = 0;

            yield return null;

            while (characterIsMovingSomewhere)
            {

                // Calculate the direction to move towards the target position
                Vector3 moveDirection = targetPos - transform.position;
                moveDirection.y = 0f; // Ensure the character moves only along the horizontal plane

                // If the remaining distance to the target position is greater than a small value
                float distanceToGoal = Vector3.Distance(transform.position, targetPos);

                if (distanceToGoal > 0.1f)
                {
                    // Normalize the direction vector to have a magnitude of 1
                    moveDirection.Normalize(); // Results in e.g. ( 0.0, 0.0, -1.0 )

                    // Move the character in the calculated direction with speed
                    if (_charCtrl == null)
                    {
                        _charCtrl = GetComponent<CharacterController>();
                    }

                    _charCtrl.Move(moveDirection * _walkingSpeed * Time.deltaTime);

                    charCtrlVelocity = _charCtrl.velocity.magnitude;
                    _playerManager.animationHandler.UpdateLocomotion(charCtrlVelocity * 0.4f);
                }
                else
                {
                    // If the character has reached the target position, exit the loop
                    yield break;
                }

                yield return null;

            }

        }

    }
}