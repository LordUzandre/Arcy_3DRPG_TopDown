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
		[SerializeField] string questGiver;
		[Header("Objectives")]
		[SerializeField] QuestObjectiveEnum questState;
		[SerializeField] QuestObjective[] objectives;
		[SerializeField] int objectiveIndex = 0;
		[Header("Requirements")]
		[SerializeField] Utils.SerializableDictionary<string, int> requirements = new Utils.SerializableDictionary<string, int>();
		[Space]
		[SerializeField] string requiredTeamMember = "";
		[SerializeField] int requiredPlayerLvl = 0;
		[SerializeField] int requiredPreviousQuestID = 0;
		[SerializeField] int requiredItemID = 0;
		[SerializeField] int requiredPreviousDialogueID = 0;
		[SerializeField] int initialDialogueID = 0;
		[SerializeField] int requiredBattleID = 0;
		[Header("Rewards")]
		Inventory.InventorySlot[] rewards;

		private void Reset()
		{
			if (requirements.Count == 0)
			{
				requirements.Add("requiredPlayerLvl", 0);
				requirements.Add("requiredTeamMember", 0);
				requirements.Add("requiredPreviousQuestID", 0);
				requirements.Add("requiredItemID", 0);
				requirements.Add("requiredPreviousDialogueID", 0);
				requirements.Add("initialDialogueID", 0);
				requirements.Add("requiredBattleID", 0);
			}
		}

		private void OnValidate()
		{
			if (questGUID == 0) questGUID = Utils.GuidGenerator.guid();
		}

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

		// private int[] requiredInts()
		// {
		// 	List<int> requirements = new List<int>();

		// 	if (requiredPlayerLvl != 0) requirements.Add(requiredPlayerLvl);
		// 	if (requiredPreviousQuestID != 0) requirements.Add(requiredPreviousQuestID);
		// 	if (requiredItemID != 0) requirements.Add(requiredItemID);
		// 	if (requiredPreviousDialogueID != 0) requirements.Add(requiredPreviousDialogueID);
		// 	if (initialDialogueID != 0) requirements.Add(initialDialogueID);
		// 	if (requiredBattleID != 0) requirements.Add(requiredBattleID);

		// 	return requirements.ToArray();
		// }

	}
}
