using System;
using UnityEngine;

namespace Arcy.Interaction
{
    public abstract class InteractibleBase : MonoBehaviour
    {
        [Header("Dialogue Block")]
        [SerializeField] public DialogueBlock dialogue;

        public abstract void Interact();
    }
}
