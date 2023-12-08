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

        private void Reset()
        {
            animator = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider>();
        }

        public override void Interact()
        {
            animator.SetTrigger("Opening");
            Debug.Log("Interaction triggered");
        }
    }
}
