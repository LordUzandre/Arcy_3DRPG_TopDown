using System.Collections;
using System.Collections.Generic;
using Arcy.Dialogue;
using UnityEngine;

namespace Arcy.Interaction
{
    public interface ISpeakable
    {
        string SpeakerName { get; set; }
        DialogueBlock[] Dialogue { get; set; }
    }

    [System.Serializable]
    public class DialogueBlock
    {
        [Header("If")]
        public bool questRelated;
        public int questGUID;
        public Quests.QuestObjectiveEnum questStatus;
        [Header("Then speak")]
        public bool singleUseDialogue = false;
        public string dialogueID;

        public string GetDialogueID()
        {
            return dialogueID;
        }
    }
}