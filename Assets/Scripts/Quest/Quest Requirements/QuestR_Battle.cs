using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Battle;

namespace Arcy.Quest
{
	[CreateAssetMenu(fileName = "Battle Requirement", menuName = "Quest/Requirements/New Battle Requirement")]
	public class QuestR_Battle : QuestRequirementBase
	{
		[SerializeField] private BattleCharacterBase _characterToDefeat;
	}
}
