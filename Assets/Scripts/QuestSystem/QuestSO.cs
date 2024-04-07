using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	[CreateAssetMenu(fileName = "new Quest", menuName = "Arcy/Quest/new Quest")]
	public class QuestSO : ScriptableObject
	{
		[SerializeField] private string _questDisplayName;
		[SerializeField] private string _questID;

		[SerializeField] private QuestStateEnum _currentQuestState;
		[Header("Objectives")]
		[SerializeField] private GameObject[] _objectives;
		[Header("Reward")]
		[SerializeField] private QuestReward _questReward;

		[Serializable]
		public class QuestReward
		{
			[SerializeField] public int coin;
			[SerializeField] public int exp;
			[SerializeField] public GameObject[] items;
		}

		public void QuestStarted()
		{
			for (int i = 0; i < _objectives.Length; i++)
			{
				if (_objectives[i].TryGetComponent<QuestObjective>(out QuestObjective objective))
				{
					if (objective.isFinished)
					{
						continue;
					}
					else
					{
						objective.InitializeObjective(_questID, i);

						if (objective.ThisObjectiveCanBeSkipped)
							continue;
						else
							return;
					}
				}
				else
				{
					Debug.LogError("No objective was found on GameObject" + _objectives[i].name);
					continue;
				}
			}
		}

		private void ObjectiveFinished(QuestObjective objective)
		{
			// Clear the current objective and Initialize the next one
			// if we've finished the final objective, finish the quest
		}
	}
}
