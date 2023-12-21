using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Interaction
{
    [RequireComponent(typeof(Animator), typeof(BoxCollider))]
    public class Door : InteractibleBase
    {
        [Header("Components")]
        [SerializeField] Animator animator;
        [SerializeField] BoxCollider boxCollider;
        private bool _isInteractible = true;
        [HideInInspector] public override bool isInteractible { get { return _isInteractible; } set { _isInteractible = value; } }

        private void Reset()
        {
            animator = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider>();
        }

        public override void Interact()
        {
            base.Interact();
            animator.SetTrigger("Opening");
            //print("Interaction triggered");
        }
    }
}
