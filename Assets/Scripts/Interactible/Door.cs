using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Interaction
{
    [RequireComponent(typeof(Animator))]
    public class Door : MonoBehaviour, InteractibleBase
    {
        [Header("Components")]
        [SerializeField] private Animator _anim;

        private bool _isInteractible = true;
        [HideInInspector] public bool isInteractible { get { return _isInteractible; } set { _isInteractible = value; } }

        [HideInInspector] public Transform ObjectTransform => transform;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _anim ??= TryGetComponent<Animator>(out Animator anim) ? anim : null;
        }
#endif

        public void Interact()
        {
            _anim.SetTrigger("Opening");
        }
    }
}
