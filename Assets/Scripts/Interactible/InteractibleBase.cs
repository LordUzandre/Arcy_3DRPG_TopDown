using System;
using UnityEngine;

namespace Arcy.Interaction
{
    public abstract class InteractibleBase : MonoBehaviour
    {
        public virtual void Interact()
        {
            throw new System.NotImplementedException();
        }
    }
}
