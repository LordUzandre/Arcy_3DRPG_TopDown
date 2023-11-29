using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Interaction
{
    public class Interactible_sign : InteractibleBase
    {
        [SerializeField] public DialogueBlock dialogue;
        public override Vector3 BubbleOffset { get; set; }

        public override void Interaction()
        {
            Debug.Log("Sign interacted with");
        }
    }
}
