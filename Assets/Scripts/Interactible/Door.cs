using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Arcy.Management;
using Arcy.Inventory;

namespace Arcy.Interaction
{
    public class Door : MonoBehaviour, InteractibleBase
    {
        [HideInInspector] public Transform ObjectTransform => transform;

        private bool _isInteractible = true;
        public bool isInteractible { get { return _isInteractible; } set { _isInteractible = value; } }

        [Header("Components")]
        [SerializeField] private Transform _doorPivot;

        [Header("Position-components")]
        [SerializeField] public Transform insidePos;
        [SerializeField] public Transform outsidePos;

        [Header("Locked door")]
        [SerializeField] public bool doorIsLocked = false;
        [SerializeField] public InventoryItem key;

        private Vector3 _ogRotation;
        private Vector3 _openRotation;
        private bool _doorIsOpen = false;

        private BoxCollider _boxCollider;

        private void Start()
        {
            _ogRotation = _doorPivot.transform.rotation.eulerAngles;
            _openRotation = _ogRotation + new Vector3(0, -80, 0);
            _boxCollider = TryGetComponent<BoxCollider>(out BoxCollider hit) ? hit : null;
        }

        private void OnEnable()
        {
            GameEventManager.instance.playerEvents.onPlayerResumeControl += MakeDoorInteractibleAgain;
        }

        private void OnDisable()
        {
            GameEventManager.instance.playerEvents.onPlayerResumeControl += MakeDoorInteractibleAgain;
        }

        public void Interact()
        {
            if (doorIsLocked)
            {
                return;
            }

            if (!_doorIsOpen)
            {
                _isInteractible = true;
                _boxCollider.enabled = false;

                Sequence mySequence = DOTween.Sequence();

                mySequence.AppendInterval(0.1f)
                .Append(_doorPivot.transform.DORotate(_openRotation, 0.7f))
                .AppendInterval(1.5f)
                .Append(_doorPivot.transform.DORotate(_ogRotation, 0.7f));
            }

            GameEventManager.instance.playerEvents.PlayerMoveToPosition(newPos());
        }

        private void MakeDoorInteractibleAgain()
        {
            _boxCollider.enabled = true;
            _doorIsOpen = ToggleBool(_doorIsOpen);
            isInteractible = true;

            bool ToggleBool(bool input)
            {
                return !input;
            }

        }

        Vector3 newPos()
        {
            Transform playerTransform = PlayerManager.instance.transform;
            Vector3 newPosition = new Vector3();

            // Calculate distances
            float distanceToInside = Vector3.Distance(playerTransform.transform.position, insidePos.transform.position);
            float distanceToOutside = Vector3.Distance(playerTransform.transform.position, outsidePos.transform.position);

            // Compare distances
            if (distanceToInside < distanceToOutside)
            {
                newPosition = outsidePos.position;
            }
            else if (distanceToOutside < distanceToInside)
            {
                newPosition = insidePos.position;
            }

            return newPosition;
        }
    }
}
