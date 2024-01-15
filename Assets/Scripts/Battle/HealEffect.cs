using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
	[CreateAssetMenu(fileName = "Heal Effect", menuName = "Battle/Effects/Heal Effect")]
	public class HealEffect : VSlice_BattleEffectBase
	{
		public int heal;
	}
}
