using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Interaction
{
    public class Door : MonoBehaviour, InteractibleBase
    {
        private bool _isInteractible = true;
        [HideInInspector] public bool isInteractible { get { return _isInteractible; } set { _isInteractible = value; } }

        [HideInInspector] public Transform ObjectTransform => transform;

        [Header("Components")]
        [SerializeField] private Animator _anim;
        [SerializeField] private Transform _doorModel;
        bool doorIsOpen = false;


#if UNITY_EDITOR
        private void OnValidate()
        {
            //_anim ??= TryGetComponent<Animator>(out Animator anim) ? _anim = anim : null;
            _anim ??= GetComponentInChildren<Animator>();
        }
#endif

        public void Interact()
        {
            if (!doorIsOpen)
            {
                _anim.SetTrigger("Opening");
                _isInteractible = true;
            }
            else
            {
                _anim.SetTrigger("Closing");
                _isInteractible = false;
            }

            Invoke(nameof(MakeDoorInteractibleAgain), 1f);
        }

        private void MakeDoorInteractibleAgain()
        {
            doorIsOpen = ToggleBool(doorIsOpen);
            isInteractible = true;

            bool ToggleBool(bool input)
            {
                return !input;
            }

        }
    }
}
