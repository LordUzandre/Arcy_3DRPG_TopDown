using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	[System.Serializable]
	public class SerializableQuest
	{
		[SerializeField] int questGUID = 0;
		[Header("UI")]
		[SerializeField] string QuestName;
		[Header("Objectives")]
		[SerializeField] QuestObjectiveEnum questState;
		[SerializeField] QuestObjective[] objectives;
		[SerializeField] int objectiveIndex = 0;
		[Header("Requirements before quest")]
		[SerializeField] int requiredPlayerLvl = 0;
		[SerializeField] int requiredPreviousQuestID = 0;
		[SerializeField] int requiredItemID = 0;
		[SerializeField] int requiredPreviousDialogueID = 0;
		[SerializeField] int requiredDialogueID = 0;
		[SerializeField] int requiredBattleID = 0;
		[Header("Rewards")]
		Inventory.InventorySlot[] rewards;

		private void OnValidate() { if (questGUID == 0) questGUID = Utils.GuidGenerator.guid(); }

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

		public void ReceiveRewards()
		{

		}

		// MARK: PRIVATE:

	}
}
