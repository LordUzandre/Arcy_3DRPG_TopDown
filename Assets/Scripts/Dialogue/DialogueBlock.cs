using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Arcy.Dialogue
{
    [Serializable]
    public class DialogueBlock
    {
        [SerializeField] string dialogueID;
        [SerializeField] UnityEvent onDialogueFinish;
    }
}
