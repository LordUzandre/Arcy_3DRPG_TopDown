using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
	public class EffectInstance
	{
		public BattleEffectBase effect;
		public int turnRemaining;

		public GameObject curActiveGameObject;
		public ParticleSystem curTickParticle;

		public EffectInstance(BattleEffectBase effect)
		{
			this.effect = effect;
			turnRemaining = effect.durationTurns;
		}
	}
}
