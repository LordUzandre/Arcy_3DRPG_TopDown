using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Dialogue;
using UnityEngine;

namespace Arcy.Interaction
{
    public abstract class SpeakingBase : InteractibleBase
    {
        [Header("Dialogue Block")]
        [SerializeField] DialogueBlock dialogue;

        public override abstract void Interact();
    }
}
