using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Quests
{
    public class Quest
    {
        // scriptable object info
        public QuestInfoSO questSO;
        // state-enum info
        public QuestStateEnum currentStatusEnum;

        // current objective Index
        private int _currentQuestObjectiveIndex;

        private QuestObjectiveState[] _questObjectiveStates;

        #region Constructors and Load/Save-Data

        /// <summary>
        /// constructors called from QuestManager using the Load/Save System
        /// </summary>

        // a new quest
        public Quest(QuestInfoSO questInfo)
        {
            questSO = questInfo;
            currentStatusEnum = QuestStateEnum.REQUIREMENTS_NOT_MET;
            _currentQuestObjectiveIndex = 0;
            _questObjectiveStates = new QuestObjectiveState[questSO.questObjectivePrefabs.Length];

            for (int i = 0; i < _questObjectiveStates.Length; i++)
            {
                _questObjectiveStates[i] = new QuestObjectiveState();
            }
        }

        // A quest from save state
        public Quest(QuestInfoSO questInfo, QuestStateEnum questState, int currentQuestObjectiveIndex, QuestObjectiveState[] questObjectiveStates)
        {
            questSO = questInfo;
            currentStatusEnum = questState;
            _currentQuestObjectiveIndex = currentQuestObjectiveIndex;
            _questObjectiveStates = questObjectiveStates;

            // if the Quest objective states and prefabs are different lengths,
            // something has changed during development and the saved data is out of sync.

            if (_questObjectiveStates.Length != questSO.questObjectivePrefabs.Length)
            {
                Debug.LogWarning($"Quest Step Prefabs and Quest Step States are "
                + "of different lengths. This indicates that something has changed. "
                + "with the QuestInfo and the saved data is now out of sync. "
                + "Reset your data - as this might cause issues. Quest id: " + this.questSO.guid);
            }
        }

        // The data of the quest that is going to get saved/loaded
        public QuestData GetQuestData()
        {
            return new QuestData(currentStatusEnum, _currentQuestObjectiveIndex, _questObjectiveStates);
        }
        #endregion

        public void AdvanceToNextObjective()
        {
            _currentQuestObjectiveIndex++;
        }

        public bool CurrentQuestObjectiveExists()
        {
            return (_currentQuestObjectiveIndex < questSO.questObjectivePrefabs.Length);
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
                questObjectivePrefab = questSO.questObjectivePrefabs[_currentQuestObjectiveIndex];
            }
            else
            {
                Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range indicating that "
                + "there's no current step: QuestId=" + questSO.displayName + ", stepIndex=" + _currentQuestObjectiveIndex);
            }

            return questObjectivePrefab;
        }

        public void StoreQuestObjectiveStatus(QuestObjectiveState questObjectiveState, int stepIndex)
        {
            if (stepIndex < _questObjectiveStates.Length)
            {
                _questObjectiveStates[stepIndex].state = questObjectiveState.state;
            }
            else
            {
                Debug.LogWarning("Tried to access quest objective data, but index was out of range: \n Quest id: " + questSO.guid + ",quest index = " + stepIndex);
            }
        }

        // Only affects UI
        public string GetFullStatusText()
        {
            string fullStatus = "";

            if (currentStatusEnum == QuestStateEnum.REQUIREMENTS_NOT_MET)
            {
                fullStatus = "Requirements are not met yet to start this quest";
            }
            else if (currentStatusEnum == QuestStateEnum.CAN_START)
            {
                fullStatus = "This Quest can be started";
            }
            else
            {
                for (int i = 0; i < _currentQuestObjectiveIndex; i++)
                {
                    fullStatus += "<s>" + _questObjectiveStates[i].status + "</s>\n";
                }

                if (currentStatusEnum == QuestStateEnum.CAN_FINISH)
                {
                    fullStatus += "The quest is ready to be turned in.";
                }
                else if (currentStatusEnum == QuestStateEnum.FINISHED)
                {
                    fullStatus += "The quest has been completed";
                }
            }
            return fullStatus;
        }
    }
}
