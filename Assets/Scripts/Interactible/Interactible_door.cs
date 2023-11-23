using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Interaction
{
    [RequireComponent(typeof(Animator), typeof(BoxCollider))]
    public class Interactible_door : InteractibleBase
    {
        [Header("Door")]
        [SerializeField] Animator animator;
        [SerializeField] BoxCollider boxCollider;

        public override Vector3 BubbleOffset { get; set; }

        private void Reset()
        {
            animator = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider>();
        }

        public void OpenDoor()
        {
            animator.SetTrigger("Opening");
            boxCollider.enabled = false;
        }

        public void ClosedDoor()
        {
            animator.SetTrigger("Closing");
            boxCollider.enabled = true;
        }

        public override void Interaction()
        {
            Debug.Log("Door interacted");
        }
    }
}
