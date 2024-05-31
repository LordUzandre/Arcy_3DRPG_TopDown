using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
	[CreateAssetMenu(fileName = "Damage Effect", menuName = "Arcy/Battle/Effects/Damage Effect SO")]
	public class DamageEffectSO : BattleEffectBase
	{
		public int damage;
	}
}
