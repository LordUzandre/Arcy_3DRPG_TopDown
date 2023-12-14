using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Interaction
{
    [RequireComponent(typeof(BoxCollider))]
    public class Sign : InteractibleBase, ISpeakable
    {
        [SerializeField] private string speakerID;
        
        public string SpeakerID
        {
            get { return speakerID; }
            set { speakerID = value; }
        }
    }
}
