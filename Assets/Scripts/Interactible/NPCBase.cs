using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Interaction
{
    public class NPCBase : SpeakingBase
    {
        public override void Interact()
        {
            if (dialogue != null)
            {
                print("NPC is speaking");
            }
            else
            {
                print("NPC doesn't have any dialogue");
            }
        }
    }
}
