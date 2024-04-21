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
		[SerializeField] public string QuestName;
		[SerializeField] public string questGiver;
		[Header("Objectives")]
		[SerializeField] public bool questStarted;
		[SerializeField] public bool questFinished;
		[SerializeField] public QuestObjectiveEnum questState;
		[SerializeField] public QuestObjective[] objectives;
		[SerializeField] public int objectiveIndex = 0;
		[Header("Requirements")]
		[SerializeField] public Utils.SerializableDictionary<string, int> requirements = new Utils.SerializableDictionary<string, int>();
		[Space]
		[SerializeField] public string requiredTeamMember = "";
		[SerializeField] public int requiredPlayerLvl = 0;
		[SerializeField] public int requiredPreviousQuestID = 0;
		[SerializeField] public int requiredItemID = 0;
		[SerializeField] public int requiredPreviousDialogueID = 0;
		[SerializeField] public int initialDialogueID = 0;
		[SerializeField] public int requiredBattleID = 0;
		[Header("Rewards")]
		[SerializeField] public Inventory.InventorySlot[] questRewards;

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
