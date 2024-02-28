using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	[CreateAssetMenu(fileName = "Inventory Requirement", menuName = "Quest/Objectives/New Inventory Objective")]
	public class QuestR_Inventory : QuestRequirementBase
	{
		/// <summary>
		/// This is a quest-related objective that means you need to have a certain thing in your inventory in order to progress.
		/// Remember: When this objective becomes the current objective in the quest, 
		/// it should check the inventory in case the item is already in the inventory.
		/// </summary>

		[Space]
		public Inventory.InventoryItemBase item;
		public int numberOfObjects = 1;

		public override void OnFinish()
		{

		}
	}
}
