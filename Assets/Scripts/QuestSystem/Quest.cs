using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Quests
{
    public class Quest : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>

        // static info
        public QuestInfoSO info;
        // state info
        public QuestState state;

        private int _currentQuestObjectiveIndex;
        private QuestObjectiveState[] _questObjectiveStates;

        #region constructors called from QuestManager

        // a new quest
        public Quest(QuestInfoSO questInfo)
        {
            this.info = questInfo;
            this.state = QuestState.REQUIREMENTS_NOT_MET;
            this._currentQuestObjectiveIndex = 0;
            this._questObjectiveStates = new QuestObjectiveState[info.questObjectivePrefabs.Length];
            for (int i = 0; i < _questObjectiveStates.Length; i++)
            {
                _questObjectiveStates[i] = new QuestObjectiveState();
            }
        }

        // A quest from save state
        public Quest(QuestInfoSO questInfo, QuestState questState, int currentQuestObjectiveIndex, QuestObjectiveState[] questObjectiveStates)
        {
            this.info = questInfo;
            this.state = questState;
            this._currentQuestObjectiveIndex = currentQuestObjectiveIndex;
            this._questObjectiveStates = questObjectiveStates;

            // if the Quest objective states and prefabs are different lengths,
            // something has changed during development and the saved data is out of sync.

            if (this._questObjectiveStates.Length != this.info.questObjectivePrefabs.Length)
            {
                Debug.LogWarning("Quest Step Prefabs and Quest Step States are "
                + "of different lengths. This indicates that something has changed. "
                + "with the QuestInfo and the saved data is now out of sync. "
                + "Reset your data - as this might cause issues. Quest id: " + this.info.guid);
            }
        }

        #endregion

        public void MoveToNextObjective()
        {
            _currentQuestObjectiveIndex++;
        }

        public bool CurrentObjectiveExists()
        {
            return (_currentQuestObjectiveIndex < info.questObjectivePrefabs.Length);
        }

        public void InstantiateCurrentQuestObjective(Transform parentTransform)
        {
            GameObject questObjectivePrefab = GetCurrentQuestObjectivePrefab();
            if (questObjectivePrefab != null)
            {
                // Use object Pooling instead of instantiation
                QuestObjective objective = UnityEngine.Object.Instantiate<GameObject>(questObjectivePrefab, parentTransform).GetComponent<QuestObjective>();
                objective.InitializeQuestObjective(info.guid, _currentQuestObjectiveIndex, _questObjectiveStates[_currentQuestObjectiveIndex].state);
            }
        }

        private GameObject GetCurrentQuestObjectivePrefab()
        {
            GameObject questObjectivePrefab = null;
            if (CurrentObjectiveExists())
            {
                questObjectivePrefab = info.questObjectivePrefabs[_currentQuestObjectiveIndex];
            }
            else
            {
                Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range indicating that "
                + "there's no current step: QuestId=" + info.guid + ", stepIndex=" + _currentQuestObjectiveIndex);
            }
            return questObjectivePrefab;
        }

        public void StoreQuestStepState(QuestObjectiveState questObjectiveState, int stepIndex)
        {
            if (stepIndex < _questObjectiveStates.Length)
            {
                _questObjectiveStates[stepIndex].state = questObjectiveState.state;
            }
            else
            {
                Debug.LogWarning("Tried to access quest objective data, but index was out of range: \n Quest id: " + info.guid + ",quest index = " + stepIndex);
            }
        }

        public QuestData GetQuestData()
        {
            return new QuestData(state, _currentQuestObjectiveIndex, _questObjectiveStates);
        }

        public string GetFullStatusText()
        {
            string fullStatus = "";

            if (state == QuestState.REQUIREMENTS_NOT_MET)
            {
                fullStatus = "Requirements are not met yet to start this quest";
            }
            else if (state == QuestState.CAN_START)
            {
                fullStatus = "This Quest can be started";
            }
            else
            {
                for (int i = 0; i < _currentQuestObjectiveIndex; i++)
                {
                    fullStatus += "<s>" + _questObjectiveStates[i].status + "</s>\n";
                }

                if (state == QuestState.CAN_FINISH)
                {
                    fullStatus += "The quest is ready to be turned in.";
                }
                else if (state == QuestState.FINISHED)
                {
                    fullStatus += "The quest has been completed";
                }
            }
            return fullStatus;
        }
    }
}
