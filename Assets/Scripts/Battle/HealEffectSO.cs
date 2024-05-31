using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
	[CreateAssetMenu(fileName = "Heal Effect", menuName = "Arcy/Battle/Effects/Heal Effect SO", order = 78)]
	public class HealEffectSO : BattleEffectBase
	{
		public int heal;
	}
}
