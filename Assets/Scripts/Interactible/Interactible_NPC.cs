using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Interaction
{
    public class Interactible_NPC : InteractibleBase
    {
        [Header("Unique to NPC")]
        [Space]
        [SerializeField] private Animator animator;
        public override Vector3 BubbleOffset { get; set; }
        //[SerializeField] private Dialogue dialogue;

        private void Reset()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }

        public override void Interaction()
        {
            Debug.Log("NPC interacted with");
        }
    }
}
