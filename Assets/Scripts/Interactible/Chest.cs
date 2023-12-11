using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Dialogue;

namespace Arcy.Interaction
{
    [RequireComponent(typeof(BoxCollider), typeof(Animator))]
    public class Chest : InteractibleBase
    {
        [SerializeField] BoxCollider boxCollider;
        [SerializeField] Animator animator;

        private void Reset()
        {
            boxCollider = GetComponent<BoxCollider>();
            animator = GetComponent<Animator>();
        }

        public override void Interact()
        {
            animator.SetTrigger("Opening");
        }
    }
}
