using System;
using UnityEngine;

namespace Arcy.Interaction
{
    public interface InteractibleBase
    {
        public Transform ObjectTransform { get; }

        public abstract bool isInteractible { get; set; }

        public abstract void Interact();
    }
}
