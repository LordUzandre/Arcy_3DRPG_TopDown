using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Arcy.Quests
{
	[CreateAssetMenu(fileName = "new Quest", menuName = "Arcy/Quest/new QuestSO")]
	public class QuestSO : ScriptableObject
	{
		[SerializeField] int questGUID = 0;
		[Header("UI")]
		[SerializeField] public string questName;
		[SerializeField] public string questGiver;
		// Requirements
		[Space]
		[SerializeField] UI.QuestRequirement[] requirementsToStartQuest;
		[Header("Objectives")]
		[SerializeField] public QuestObjectiveEnum questState;
		[SerializeField] public bool questStarted;
		[SerializeField] public bool questFinished;
		[SerializeField] public QuestObjective[] objectives;
		[SerializeField] public int objectiveIndex = 0;
		[Header("Rewards")]
		[SerializeField] public Inventory.InventorySlot[] questRewards;

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (questGUID == 0) questGUID = Utils.GuidGenerator.guid();
			if (questName != name) questName = name;
		}
#endif

		// MARK: PUBLIC: 

		public bool CheckRequirementsMet(int providedID)
		{
			return providedID == questGUID;
		}

		public void BeginQuest()
		{
			objectiveIndex = 1;
			objectives[objectiveIndex].InitializeObjective();
		}

		public void AdvanceToNextObjective()
		{
			// When we have finished the last objective
			if (objectiveIndex > objectives.Length)
			{
				FinishQuest();
				return;
			}

			objectives[objectiveIndex].FinishObjective();
			objectiveIndex++;
			objectives[objectiveIndex].InitializeObjective();
		}

		public void FinishQuest()
		{
		}

		// MARK: PRIVATE:

	}

}
