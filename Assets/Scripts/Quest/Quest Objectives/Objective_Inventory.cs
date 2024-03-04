using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Inventory;

namespace Arcy.Quests
{
	[CreateAssetMenu(fileName = "Inventory Requirement", menuName = "Quest/Objectives/New Inventory Objective")]
	public class Objective_Inventory : QuestObjectiveBase
	{
		/// <summary>
		/// This is a quest-related objective that means you need to have a certain thing in your inventory in order to progress.
		/// Remember: When this objective becomes the current objective in the quest, 
		/// it should check the inventory in case the item is already in the inventory.
		/// </summary>

		[SerializeField] private InventoryItemBase _item;
		// [SerializeField] int _numberOfObjects = 1;
		[SerializeField] private bool thisObjectiveCanBeSkipped;
		public override bool ThisObjectiveCanBeSkipped { get { return thisObjectiveCanBeSkipped; } set { thisObjectiveCanBeSkipped = value; } }

		public override event Action<QuestObjectiveBase> ObjectiveFinished;


		private void inventoryUpdated(InventoryItemBase item)
		{
			if (item == _item) // TODO: Also check whether the inventory is >= _numberOfObjects
			{

			}
		}

		public override void ObjectiveActivate()
		{
		}

		public override void OnFinish()
		{

		}
	}
}
