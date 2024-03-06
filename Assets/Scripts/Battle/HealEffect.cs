using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
	[CreateAssetMenu(fileName = "Heal Effect", menuName = "Battle/Effects/Heal Effect", order = 220)]
	public class HealEffect : BattleEffectBase
	{
		public int heal;
	}
}
