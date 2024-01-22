using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Dialogue;

namespace Arcy.Interaction
{
    [RequireComponent(typeof(BoxCollider), typeof(Animator))]
    public class Chest : MonoBehaviour, InteractibleBase
    {
        [SerializeField] BoxCollider _boxCollider;
        [SerializeField] Animator _anim;
        [HideInInspector] public Transform ObjectTransform => transform;

        private bool _isInteractible = true;
        [HideInInspector] public bool isInteractible { get { return _isInteractible; } set { _isInteractible = value; } }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _boxCollider ??= TryGetComponent<BoxCollider>(out BoxCollider boxCollider) ? _boxCollider = boxCollider : null;
            _anim ??= TryGetComponent<Animator>(out Animator anim) ? _anim = anim : null;
        }
#endif

        public void Interact()
        {
            _anim.SetTrigger("Opening");
        }
    }
}
