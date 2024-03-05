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

        public Quest(QuestInfoSO questInfo)
        {
            this.info = questInfo;
            this.state = QuestState.REQUIREMENTS_NOT_MET;
            this.currentQuestObjectiveIndex = 0;
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
                objective.InitializeQuestObjective(info.id);
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
    }
}
