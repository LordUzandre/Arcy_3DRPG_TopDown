using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest")]
    public class Quest : ScriptableObject
    {
        [field: SerializeField] public string id { get; private set; }

        [Header("Icons")]
        public Image questIcon;
        public Image questGiverIcon;
        public Image questLocationIcon;
        [Header("Important parts")]
        public string questDisplayNAme;
        public string questGiver;
        public string questLocation;
        [Space]
        [Header("Quest Requirements")]
        [SerializeField] private List<QuestPrerequisite> preRequisite;
        [Header("Objectives")]
        public QuestObjectiveBase[] questObjectives;
        [Header("Rewards on finish")]
        public Inventory.InventoryItemBase[] rewards;

        [Header("Bools")]
        public bool isAvailable = false;
        public bool inProgress = false;
        public bool isFinished = false;

        public Action<Quest> ObjectiveCompletedAction;

        // Ensure that the id is always the same as the Scriptable Object Asset
        private void OnValidate()
        {
#if UNITY_EDITOR
            id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        // Pre-requisites

        // Triggered by QuestManager when either the game starts, or when the quest is started in-game
        public void Startup()
        {
            questObjectives[0].ObjectiveActivate();
            questObjectives[0].ObjectiveFinished += ObjectiveCompleted;
        }

        // Tick off finished objectives and open the next one in the list
        #region Progress
        private void ObjectiveCompleted(QuestObjectiveBase objective)
        {

        }
        #endregion

        // Reward
        // Add rewards to inventory, clean up quest log and/or progress story

    }
}
