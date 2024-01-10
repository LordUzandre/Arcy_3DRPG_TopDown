using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
	public class EffectInstance
	{
		public VSlice_BattleEffectBase effect;
		public int turnRemaining;

		public GameObject curActiveGameObject;
		public ParticleSystem curTickParticle;

		public EffectInstance(VSlice_BattleEffectBase effect)
		{
			this.effect = effect;
			turnRemaining = effect.durationTurns;
		}
	}
}
