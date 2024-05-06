using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
    [CreateAssetMenu(fileName = "Quest Info", menuName = "Arcy/Quest/Quest Info", order = 63)]
    public class QuestInfoSO : ScriptableObject
    {
        /// <summary>
        /// Every quest should be their own script/class, and be accompanied by one of these.
        /// These hold the requirements, the rewards and acts as a container for all the objectives.
        /// </summary>

        [field: SerializeField] public int guid { get; private set; }

        [Header("General")]
        public string displayName;

        [Header("Requirements")]
        public int levelRequirement;
        public QuestInfoSO[] questPrerequisites;

        [Header("Objectives")]
        public GameObject[] questObjectivePrefabs;

        [Header("Rewards")]
        public Inventory.InventorySlot[] rewardItems;

        // ensure the id is always the name of the Scriptable Object asset
        private void OnValidate()
        {
#if UNITY_EDITOR
            if (guid == 0) guid = Utils.GuidGenerator.guid(this);
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

    }
}
