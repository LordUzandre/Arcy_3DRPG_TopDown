using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quest
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest")]
    public class Quest : ScriptableObject
    {
        public Sprite questIcon;
        public Sprite questGiverIcon;
        public Sprite questLocationIcon;

        public string questName;
        public string questGiver;
        public string questLocation;

        // Pre-requisites

        // Progress

        // Reward

    }
}
