using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
	[CreateAssetMenu(fileName = "Damage Effect", menuName = "Battle/Effects/Damage Effect")]
	public class DamageEffect : VSlice_BattleEffectBase
	{
		public int damage;
	}
}
