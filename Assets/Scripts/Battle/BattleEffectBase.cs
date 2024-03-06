using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
	public abstract class BattleEffectBase : ScriptableObject
	{
		public int durationTurns;

		[Header("Prefabs")]
		public GameObject activePrefab;
		public GameObject tickPrefab;
	}
}
