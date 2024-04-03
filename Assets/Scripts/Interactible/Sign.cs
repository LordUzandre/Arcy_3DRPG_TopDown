using System.Collections;
using System.Collections.Generic;
using Arcy.Dialogue;
using UnityEngine;

namespace Arcy.Interaction
{
    [RequireComponent(typeof(BoxCollider))]
    public class Sign : MonoBehaviour, InteractibleBase, ISpeakable
    {
        [SerializeField] private string speakerID;
        public string SpeakerID { get { return speakerID; } set { speakerID = value; } }

        private DialogueBlock _dialogue;
        public DialogueBlock Dialogue { get { return _dialogue; } set { _dialogue = value; } }

        private bool _isInteractible = true;
        public bool isInteractible { get { return _isInteractible; } set { _isInteractible = value; } }

        [HideInInspector] public Transform ObjectTransform => transform;

        public void Interact() { }
    }
}
