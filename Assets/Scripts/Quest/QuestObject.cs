using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quest
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest")]
    public class QuestObject : ScriptableObject
    {
        [Header("quest index")]
        public string questIndex;

        [Header("Icons")]
        public Sprite questIcon;
        public Sprite questGiverIcon;
        public Sprite questLocationIcon;

        [Header("Important parts")]
        public string questTitle;
        public string questGiver;
        public string questLocation;
        [TextArea(4, 10)]
        public string[] description;
        [Header("Requirements")]
        public QuestRequirementBase[] steps;
        [Header("Reward")]
        public Inventory.InventoryItemBase[] rewards;

        [Header("Bools")]
        public bool isAvailable;
        public bool inProgress;
        public bool isFinished;


        // Pre-requisites

        // Progress

        // Reward

    }
}
