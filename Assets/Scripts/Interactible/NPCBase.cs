using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Animation;
using Arcy.Dialogue;

namespace Arcy.Interaction
{
    public class NPCBase : InteractibleBase
    {
        [Header("Animation Handler")]
        [SerializeField] private NPCAnimationHandler _npcAnimationHandler;

        private void OnEnable()
        {
            if (_npcAnimationHandler == null)
            {
                _npcAnimationHandler = GetComponent<NPCAnimationHandler>();
            }
        }

        public override void Interact()
        {
            if (dialogue != null)
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
