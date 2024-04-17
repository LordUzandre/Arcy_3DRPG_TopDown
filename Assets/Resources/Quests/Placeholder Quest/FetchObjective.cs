using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Inventory;
using Arcy.Management;
using UnityEngine;

namespace Arcy.Quests
{
	public class FetchObjective : QuestObjective
	{
		private bool _thisObjectiveCanBeSkipped;
		public override bool ThisObjectiveCanBeSkipped
		{
			get { return _thisObjectiveCanBeSkipped; }
			set { _thisObjectiveCanBeSkipped = value; }
		}

		[SerializeField] private InventoryItem _item;
		// [SerializeField] private int requiredAmount = 1;

		public override void InitializeObjective(int questId, int objectiveIndex)
		{
			base.InitializeObjective(questId, objectiveIndex);

			InventoryUpdated();

			Management.GameManager.instance.gameEventManager.inventoryEvents.onInventoryUpdated += InventoryUpdated;
		}

		private void InventoryUpdated()
		{

		}

	}
}
