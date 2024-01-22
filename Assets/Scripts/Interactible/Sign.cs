using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Interaction
{
    [RequireComponent(typeof(BoxCollider))]
    public class Sign : MonoBehaviour, InteractibleBase, ISpeakable
    {
        [SerializeField] private string speakerID;
        private bool _isInteractible = true;
        [HideInInspector] public Transform ObjectTransform => transform;

        [HideInInspector] public bool isInteractible { get { return _isInteractible; } set { _isInteractible = value; } }

        public string SpeakerID { get { return speakerID; } set { speakerID = value; } }

        public void Interact() { }
    }
}
