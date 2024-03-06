using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Quests
{
    public class Quest : MonoBehaviour
    {
        // static info
        public QuestInfoSO info;

        // state info
        public QuestState state;

        private int currentQuestObjectiveIndex;
        private QuestObjectiveState[] questObjectiveStates;

        public Quest(QuestInfoSO questInfo)
        {
            this.info = questInfo;
            this.state = QuestState.REQUIREMENTS_NOT_MET;
            this.currentQuestObjectiveIndex = 0;
            this.questObjectiveStates = new QuestObjectiveState[info.questObjectivePrefabs.Length];
            for (int i = 0; i < questObjectiveStates.Length; i++)
            {
                questObjectiveStates[i] = new QuestObjectiveState();
            }
        }

        public Quest(QuestInfoSO questInfo, QuestState questState, int currentQuestObjectiveIndex, QuestObjectiveState[] questObjectiveStates)
        {
            this.info = questInfo;
            this.state = questState;
            this.currentQuestObjectiveIndex = currentQuestObjectiveIndex;
            this.questObjectiveStates = questObjectiveStates;

            // if the Quest objective states and prefabs are different lengths,
            // something has changed during development and the saved data is out of sync.

            if (this.questObjectiveStates.Length != this.info.questObjectivePrefabs.Length)
            {
                Debug.LogWarning("Quest Step Prefabs and Quest Step States are "
                + "of different lengths. This indicates that something has changed. "
                + "with the QuestInfo and the saved data is now out of sync. "
                + "Reset your data - as this might cause issues. Quest id: " + this.info.id);
            }
        }

        public void MoveToNextObjective()
        {
            currentQuestObjectiveIndex++;
        }

        public bool CurrentObjectiveExists()
        {
            return (currentQuestObjectiveIndex < info.questObjectivePrefabs.Length);
        }

        public void InstantiateCurrentQuestObjective(Transform parentTransform)
        {
            GameObject questObjectivePrefab = GetCurrentQuestObjectivePrefab();
            if (questObjectivePrefab != null)
            {
                // Use object Pooling instead of instantiation
                QuestObjective objective = UnityEngine.Object.Instantiate<GameObject>(questObjectivePrefab, parentTransform).GetComponent<QuestObjective>();
                objective.InitializeQuestObjective(info.id, currentQuestObjectiveIndex, questObjectiveStates[currentQuestObjectiveIndex].state);
            }
        }

        private GameObject GetCurrentQuestObjectivePrefab()
        {
            GameObject questObjectivePrefab = null;
            if (CurrentObjectiveExists())
            {
                questObjectivePrefab = info.questObjectivePrefabs[currentQuestObjectiveIndex];
            }
            else
            {
                Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range indicating that "
                + "there's no current step: QuestId=" + info.id + ", stepIndex=" + currentQuestObjectiveIndex);
            }
            return questObjectivePrefab;
        }

        public void StoreQuestStepState(QuestObjectiveState questObjectiveState, int stepIndex)
        {
            if (stepIndex < questObjectiveStates.Length)
            {
                questObjectiveStates[stepIndex].state = questObjectiveState.state;
            }
            else
            {
                Debug.LogWarning("Tried to access quest objective data, but index was out of range: \n Quest id: " + info.id + ",quest index = " + stepIndex);
            }
        }

        public QuestData GetQuestData()
        {
            return new QuestData(state, currentQuestObjectiveIndex, questObjectiveStates);
        }
    }
}
