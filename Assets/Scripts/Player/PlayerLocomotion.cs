using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.InputManagement;
using Arcy.Management;
using DG.Tweening;

namespace Arcy.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerLocomotion : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private CharacterController _charCtrl;

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
        public void HandleAllMovement(float delta)
        {
            Vector3 moveDirection = GetYourBearings();

            HandleGroundedMovement(delta, moveDirection);
            HandleRotation(delta, moveDirection);
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
            if (_charCtrl.isGrounded)
            {
                _velocity.y = 0;
            }
            else
            {
                _velocity.y = -_gravity;
            }

            _charCtrl.Move(_velocity);
            _playerManager.animationHandler.locomotion = _moveAmount;
            _playerManager.animationHandler.UpdateLocomotion();
        }

        private void HandleRotation(float delta, Vector3 targetRotationDirection)
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
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, _rotationSpeed * delta);
            transform.rotation = targetRotation;
        }

        public void MoveToSpecificPosition(Vector3 newPos)
        {
            Vector3 newPosRedux = new Vector3(newPos.x, transform.position.y, newPos.z);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOLookAt(newPosRedux, 0.4f).SetEase(Ease.InOutSine))
            .Append(transform.DOMove(newPosRedux, 0.8f).SetEase(Ease.InOutSine))
            .OnComplete(() => GameManager.instance.gameEventManager.playerEvents.PlayerResumeControl());
        }
    }
}