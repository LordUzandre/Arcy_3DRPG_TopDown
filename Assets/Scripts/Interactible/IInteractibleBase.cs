using System;
using UnityEngine;

namespace Arcy.Interaction
{
    public interface IInteractibleBase
    {
        public Transform ObjectTransform { get; } // Used by InteractionIcon

        public abstract bool isInteractible { get; set; }

        public abstract void Interact();
    }
}
