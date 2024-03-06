using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
	[CreateAssetMenu(fileName = "Damage Effect", menuName = "Battle/Effects/Damage Effect", order = 210)]
	public class DamageEffect : BattleEffectBase
	{
		public int damage;
	}
}
