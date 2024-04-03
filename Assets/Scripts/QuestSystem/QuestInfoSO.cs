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

        [field: SerializeField] public string guid { get; private set; }

        [Header("General")]
        [SerializeField] public string displayName;

        [Header("Requirements")]
        [SerializeField] public QuestInfoSO[] questPrerequisites; // Other finished quests required
        [SerializeField] public int[] dialoguePrerequisites; // Dialogue prerequirement
        [SerializeField] public Inventory.InventoryItem[] item; // Inventory item

        [Header("Objectives")]
        [SerializeField] public GameObject[] questObjectivePrefabs;

        [Header("Rewards")]
        [SerializeField] public Inventory.InventorySlot[] rewardItems;

        // Generate a unique identifier
        [ContextMenu("Generate Unique Identifier (guid)")]
        private void GenerateGuid() { guid = System.Guid.NewGuid().ToString(); }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // id = this.name;
            if (guid == null)
            {
                guid = System.Guid.NewGuid().ToString();
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

    }
}
