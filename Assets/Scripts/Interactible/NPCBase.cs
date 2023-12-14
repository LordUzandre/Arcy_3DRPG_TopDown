using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Animation;
using Arcy.Dialogue;

namespace Arcy.Interaction
{
    public class NPCBase : InteractibleBase, ISpeakable
    {
        [Header("Dialogue")]
        [SerializeField] private string _speakerID;
        [Header("Animation Handler")]
        [SerializeField] private NPCAnimationHandler _npcAnimationHandler;

        public string SpeakerID
        {
            get { return _speakerID; }
            set { _speakerID = value; }
        }

        private void OnEnable()
        {
            if (_npcAnimationHandler == null)
            {
                _npcAnimationHandler = GetComponent<NPCAnimationHandler>();
            }
        }

        public override void Interact()
        {
            if (_speakerID != null)
            {
                _npcAnimationHandler.anim.SetTrigger("talking");
            }
            else
            {
                print("NPC doesn't have any dialogue");
            }
        }
    }
}
