using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
    [CreateAssetMenu(fileName = "Quest Info", menuName = "Quest/Quest Info", order = 110)]
    public class QuestInfoSO : ScriptableObject
    {
        /// <summary>
        /// Every quest should be their own script/class, but be accompanied by one of these
        /// </summary>

        [field: SerializeField] public string id { get; private set; }
        [field: SerializeField] public string guid { get; private set; }

        [Header("General")]
        [SerializeField] public string displayName;

        [Header("Requirements")]
        [SerializeField] public int levelRequirement; // does the quest require a minimum level to accessible?
        [SerializeField] public QuestInfoSO[] questPrerequisites; // Do other Quests need to be finished before this can accessed?

        [Header("Objectives")]
        [SerializeField] public GameObject[] questObjectivePrefabs;

        [Header("Rewards")]
        [SerializeField] public int goldReward;
        [SerializeField] public int experienceReward;

        [ContextMenu("Generate Unique Identifier")]
        private void GenerateGuid()
        {
            // Generate a unique identifier
            guid = System.Guid.NewGuid().ToString();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Ensure that the id is always the same as the Scriptable Object Asset
            id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

    }
}
