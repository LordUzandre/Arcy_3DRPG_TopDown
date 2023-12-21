using System;
using UnityEngine;

namespace Arcy.Interaction
{
    public abstract class InteractibleBase : MonoBehaviour
    {
        public abstract bool isInteractible { get; set; }

        public virtual void Interact()
        {
            isInteractible = false;
        }
    }
}
