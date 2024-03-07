using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Management;
using UnityEngine;

namespace Arcy.Quests
{
	public class PlaceholderQuestObjective : QuestObjective
	{
		private bool _thisObjectiveCanBeSkipped;
		public override bool ThisObjectiveCanBeSkipped { get { return _thisObjectiveCanBeSkipped; } set { value = _thisObjectiveCanBeSkipped; } }

		[SerializeField] private Inventory.InventoryItemBase _item;

		private string _status = "Visit the 1st pillar";

		private void Start()
		{
			UpdateState();
		}

		private void UpdateState()
		{
			string state = _item.name.ToString();
			string status = "you have foundthe item: " + state;
			ChangeState(state, status);
		}

		public override void ObjectiveActivate()
		{
			GameEventManager.instance.inventoryEvents.onInventoryItemAdded += ItemAddedToInventory;
		}

		public override void OnFinish()
		{
			GameEventManager.instance.inventoryEvents.onInventoryItemAdded -= ItemAddedToInventory;
		}

		protected override void SetQuestObjectiveState(string state)
		{
			throw new NotImplementedException();
		}

		private void ItemAddedToInventory(Inventory.InventoryItemBase item)
		{
			if (item == _item)
			{
				Debug.Log("Quest: You found the requested item");
			}
		}
	}
}
