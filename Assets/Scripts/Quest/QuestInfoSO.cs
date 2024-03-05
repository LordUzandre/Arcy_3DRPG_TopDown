using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
    public class QuestInfoSO : ScriptableObject
    {
        [field: SerializeField] public string id { get; private set; }

        [Header("General")]
        [SerializeField] public string displayName;

        [Header("Requirements")]
        public int levelRequirement;
        public QuestInfoSO[] questPrerequisites;

        [Header("Steps")]
        public GameObject[] questObjectivePrefabs;

        [Header("Rewards")]
        public int goldReward;
        public int experienceReward;

        // Ensure that the id is always the same as the Scriptable Object Asset
#if UNITY_EDITOR
        private void OnValidate()
        {
            id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

    }
}
