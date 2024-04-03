using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Dialogue;
using UnityEngine;

namespace Arcy.Interaction
{
    public interface ISpeakable
    {
        string SpeakerID { get; set; }
        DialogueBlock Dialogue { get; set; }
    }
}
