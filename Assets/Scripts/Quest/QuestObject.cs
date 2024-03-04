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
        [Space]
        [Header("Objectives")]
        public List<QuestObjectiveBase> questObjectives;
        public List<QuestObjectiveBase> completedObjectives;
        [Header("Rewards on finish")]
        public Inventory.InventoryItemBase[] rewards;

        [Header("Bools")]
        public bool isAvailable = false;
        public bool inProgress = false;
        public bool isFinished = false;

        public Action<QuestObject> ObjectiveCompletedAction;

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
            // Find the index of the objective in myList
            int objectiveIndex = questObjectives.IndexOf(objective);

            // If the objective is not found, return
            if (objectiveIndex < 0)
                return;

            // Create a new list to store the elements to be removed
            List<QuestObjectiveBase> removedList = new List<QuestObjectiveBase>();

            // Iterate through the elements preceding the objective and add them to the removedList
            for (int i = 0; i < objectiveIndex; i++)
            {
                removedList.Add(questObjectives[i]);
            }

            questObjectives.RemoveRange(0, objectiveIndex); // Remove elements from index 0 to index, in case there are objectives that can be skipped ahead of it


            if (ObjectiveCompletedAction != null)
            {
                ObjectiveCompletedAction(this);
            }

            // Make the top objective in the list the current one.
            questObjectives[0].ObjectiveActivate();
            if (questObjectives[0].ThisObjectiveCanBeSkipped)
            {
                questObjectives[1].ObjectiveActivate();
            }

            Debug.Log("akjfhajergaeirghahfijawhfuihksvsekjgheiuhgjiehgaigih");
        }
        #endregion

        // Reward
        // Add rewards to inventory, clean up quest log and/or progress story

    }
}
