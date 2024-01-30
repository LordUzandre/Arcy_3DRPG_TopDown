using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Arcy.Interaction
{
    public class Door : MonoBehaviour, InteractibleBase
    {
        [HideInInspector] public Transform ObjectTransform => transform;

        private bool _isInteractible = true;
        [HideInInspector]
        public bool isInteractible
        {
            get { return _isInteractible; }
            set { _isInteractible = value; }
        }

        [Header("Components")]
        [SerializeField] private Transform _doorPivot;
        private Vector3 _ogRotation;
        private Vector3 _openRotation;
        private bool _doorIsOpen = false;

        private void Start()
        {
            _ogRotation = _doorPivot.transform.rotation.eulerAngles;
            _openRotation = _ogRotation + new Vector3(0, -80, 0);
        }

        public void Interact()
        {
            if (!_doorIsOpen)
            {
                _isInteractible = true;
                _doorPivot.transform.DORotate(_openRotation, 1f);
            }
            else
            {
                _isInteractible = false;
                _doorPivot.transform.DORotate(_ogRotation, 1f);
            }

            Invoke(nameof(MakeDoorInteractibleAgain), 1f);
        }

        private void MakeDoorInteractibleAgain()
        {
            _doorIsOpen = ToggleBool(_doorIsOpen);
            isInteractible = true;

            bool ToggleBool(bool input)
            {
                return !input;
            }

        }
    }
}
