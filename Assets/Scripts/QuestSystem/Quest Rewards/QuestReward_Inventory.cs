using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Inventory;

namespace Arcy.Quests
{
	[CreateAssetMenu(fileName = "Inventory Reward", menuName = "Arcy/Quest/Rewards/Inventory Reward", order = 65)]
	[Serializable]
	public class QuestReward_Inventory : QuestRewards
	{
		[SerializeField] public InventoryItem rewardItem;
		[SerializeField] public int amount;
	}
}
