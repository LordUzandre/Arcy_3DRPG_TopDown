using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest")]
    public class QuestObject : ScriptableObject
    {
        [Header("quest index")]
        public string questIndex;

        [Header("Icons")]
        public Image questIcon;
        public Image questGiverIcon;
        public Image questLocationIcon;
        [Header("Important parts")]
        public string questTitle;
        public string questGiver;
        public string questLocation;
        [Header("Requirements")]
        public List<QuestRequirementBase> questObjectives;
        [Header("Rewards on finish")]
        public Inventory.InventoryItemBase[] rewards;

        [Header("Bools")]
        public bool isAvailable = false;
        public bool inProgress = false;
        public bool isFinished = false;

        // Pre-requisites

        // Progress
        //Tick off finished steps and open next step

        // Reward
        // Add rewards to inventory, clean up quest log and/or progress story

    }
}
