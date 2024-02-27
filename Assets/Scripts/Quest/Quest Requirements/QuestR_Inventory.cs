using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	[CreateAssetMenu(fileName = "Inventory Requirement", menuName = "Quest/Requirements/New Inventory Requirement")]
	public class QuestR_Inventory : QuestRequirementBase
	{
		public int numberOfObjects = 1;

		public override void OnFinish()
		{

		}
	}
}
