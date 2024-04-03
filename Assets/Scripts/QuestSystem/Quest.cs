using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Quests
{
    public class Quest
    {
        // static info
        public QuestInfoSO infoSO;
        // state-enum info
        public QuestStateEnum statusEnum;

        // current objective Index
        private int _currentQuestObjectiveIndex;

        private QuestObjectiveState[] _questObjectiveStates;

        #region constructors called from QuestManager

        // a new quest
        public Quest(QuestInfoSO questInfo)
        {
            infoSO = questInfo;
            statusEnum = QuestStateEnum.REQUIREMENTS_NOT_MET;
            _currentQuestObjectiveIndex = 0;
            _questObjectiveStates = new QuestObjectiveState[infoSO.questObjectivePrefabs.Length];

            for (int i = 0; i < _questObjectiveStates.Length; i++)
            {
                _questObjectiveStates[i] = new QuestObjectiveState();
            }
        }

        // A quest from save state
        public Quest(QuestInfoSO questInfo, QuestStateEnum questState, int currentQuestObjectiveIndex, QuestObjectiveState[] questObjectiveStates)
        {
            infoSO = questInfo;
            statusEnum = questState;
            _currentQuestObjectiveIndex = currentQuestObjectiveIndex;
            _questObjectiveStates = questObjectiveStates;

            // if the Quest objective states and prefabs are different lengths,
            // something has changed during development and the saved data is out of sync.

            if (_questObjectiveStates.Length != infoSO.questObjectivePrefabs.Length)
            {
                Debug.LogWarning($"Quest Step Prefabs and Quest Step States are "
                + "of different lengths. This indicates that something has changed. "
                + "with the QuestInfo and the saved data is now out of sync. "
                + "Reset your data - as this might cause issues. Quest id: " + this.infoSO.guid);
            }
        }

        #endregion

        public void AdvanceToNextObjective()
        {
            _currentQuestObjectiveIndex++;
        }

        public bool CurrentObjectiveExists()
        {
            return (_currentQuestObjectiveIndex < infoSO.questObjectivePrefabs.Length);
        }

        public void InstantiateCurrentQuestObjective(Transform parentTransform)
        {
            GameObject questObjectivePrefab = GetCurrentQuestObjectivePrefab();

            if (questObjectivePrefab != null)
            {
                QuestObjective objective = UnityEngine.Object.Instantiate<GameObject>(questObjectivePrefab, parentTransform).GetComponent<QuestObjective>();
                objective.Initialize(infoSO.guid, _currentQuestObjectiveIndex, _questObjectiveStates[_currentQuestObjectiveIndex].state);
            }
        }

        private GameObject GetCurrentQuestObjectivePrefab()
        {
            GameObject questObjectivePrefab = null;

            if (CurrentObjectiveExists())
            {
                questObjectivePrefab = infoSO.questObjectivePrefabs[_currentQuestObjectiveIndex];
            }
            else
            {
                Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range indicating that "
                + "there's no current step: QuestId=" + infoSO.displayName + ", stepIndex=" + _currentQuestObjectiveIndex);
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
                Debug.LogWarning("Tried to access quest objective data, but index was out of range: \n Quest id: " + infoSO.guid + ",quest index = " + stepIndex);
            }
        }

        public QuestData GetQuestData()
        {
            return new QuestData(statusEnum, _currentQuestObjectiveIndex, _questObjectiveStates);
        }

        // Only affects UI
        public string GetFullStatusText()
        {
            string fullStatus = "";

            if (statusEnum == QuestStateEnum.REQUIREMENTS_NOT_MET)
            {
                fullStatus = "Requirements are not met yet to start this quest";
            }
            else if (statusEnum == QuestStateEnum.CAN_START)
            {
                fullStatus = "This Quest can be started";
            }
            else
            {
                for (int i = 0; i < _currentQuestObjectiveIndex; i++)
                {
                    fullStatus += "<s>" + _questObjectiveStates[i].status + "</s>\n";
                }

                if (statusEnum == QuestStateEnum.CAN_FINISH)
                {
                    fullStatus += "The quest is ready to be turned in.";
                }
                else if (statusEnum == QuestStateEnum.FINISHED)
                {
                    fullStatus += "The quest has been completed";
                }
            }
            return fullStatus;
        }
    }
}
