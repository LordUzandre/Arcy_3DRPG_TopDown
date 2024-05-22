using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Quests
{
    public class Quest
    {
        public QuestSO QuestObject; // scriptable object info
        public QuestObjectiveEnum CurrentStatusEnum; // state-enum info

        public int currentQuestObjectiveIndex; // current objective Index
        // private QuestObjectiveState[] _questObjectiveStates;

        public void AdvanceToNextObjective()
        {
            currentQuestObjectiveIndex++;
        }

        public bool CurrentQuestObjectiveExists()
        {
            return currentQuestObjectiveIndex < QuestObject.objectives.Length;
        }

        public void InstantiateCurrentQuestObjective(Transform parentTransform)
        {
            GameObject questObjectivePrefab = GetCurrentQuestObjectivePrefab();

            if (questObjectivePrefab != null)
            {
                QuestObjective objective = UnityEngine.Object.Instantiate<GameObject>(questObjectivePrefab, parentTransform).GetComponent<QuestObjective>();
                // objective.InitializeObjective(questSO.guid, _currentQuestObjectiveIndex, _questObjectiveStates[_currentQuestObjectiveIndex].state);
            }
        }

        private GameObject GetCurrentQuestObjectivePrefab()
        {
            GameObject questObjectivePrefab = null;

            if (CurrentQuestObjectiveExists())
            {
                // TODO: Initialize the next objective in the QuestSO
                // questObjectivePrefab = QuestObject.objectives[_currentQuestObjectiveIndex];
            }
            else
            {
                Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range indicating that "
                + "there's no current step: QuestId=" + QuestObject.questName + ", stepIndex=" + currentQuestObjectiveIndex);
            }

            return questObjectivePrefab;
        }

        // Only affects UI
        public string GetFullStatusText()
        {
            string fullStatus = "";
            // if (CurrentStatusEnum == QuestObjectiveEnum.REQUIREMENTS_NOT_MET)
            // {
            //     fullStatus = "Requirements are not met yet to start this quest";
            // }
            // else if (CurrentStatusEnum == QuestObjectiveEnum.CAN_START)
            // {
            //     fullStatus = "This Quest can be started";
            // }
            // else
            // {
            //     for (int i = 0; i < _currentQuestObjectiveIndex; i++)
            //     {
            //         fullStatus += "<s>" + _questObjectiveStates[i].Status + "</s>\n";
            //     }

            //     if (CurrentStatusEnum == QuestObjectiveEnum.CAN_FINISH)
            //     {
            //         fullStatus += "The quest is ready to be turned in.";
            //     }
            //     else if (CurrentStatusEnum == QuestObjectiveEnum.FINISHED)
            //     {
            //         fullStatus += "The quest has been completed";
            //     }
            // }
            return fullStatus;
        }

        // MARK: SAVE/LOAD:

        /// <summary>
        /// constructors called from QuestManager using the Load/Save System
        /// </summary>

        // a new quest
        public Quest(QuestSO questInfo)
        {
            this.QuestObject = questInfo;
            CurrentStatusEnum = QuestObjectiveEnum.REQUIREMENTS_NOT_MET;
            currentQuestObjectiveIndex = 0;
            // _questObjectiveStates = new QuestObjectiveState[this.QuestObject.objectives.Length];

            // for (int i = 0; i < _questObjectiveStates.Length; i++)
            // {
            //     _questObjectiveStates[i] = new QuestObjectiveState();
            // }
        }

        // A quest from save state
        public Quest(QuestSO questInfo, QuestObjectiveEnum questState, int currentQuestObjectiveIndex) //, QuestObjectiveState[] questObjectiveStates)
        {
            this.QuestObject = questInfo;
            CurrentStatusEnum = questState;
            this.currentQuestObjectiveIndex = currentQuestObjectiveIndex;
            // _questObjectiveStates = questObjectiveStates;

            // if the Quest objective states and prefabs are different lengths,
            // something has changed during development and the saved data is out of sync.

            // if (_questObjectiveStates.Length != this.QuestObject.objectives.Length)
            // {
            //     Debug.LogWarning($"Quest Step Prefabs and Quest Step States are "
            //     + "of different lengths. This indicates that something has changed. "
            //     + "with the QuestSO and the saved data is now out of sync. "
            //     + "Reset your data - as this might cause issues. Quest id: " + this.QuestObject.questGuid);
            // }
        }

        public void StoreQuestObjectiveStatus(QuestObjectiveState questObjectiveState, int objectiveIndex)
        {
            // if (objectiveIndex < _questObjectiveStates.Length)
            // {
            //     _questObjectiveStates[objectiveIndex].State = questObjectiveState.State;
            // }
            // else
            // {
            //     Debug.LogWarning("Tried to access quest objective data, but index was out of range: \n Quest id: " + QuestObject.questGuid + ",quest index = " + objectiveIndex);
            // }
        }

        // The data of the quest that is going to get saved/loaded
        public QuestSaveData GetQuestData()
        {
            return new QuestSaveData(CurrentStatusEnum, currentQuestObjectiveIndex); //, _questObjectiveStates);
        }

    }
}
