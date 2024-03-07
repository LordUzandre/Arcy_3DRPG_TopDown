using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Effect Combat Action", menuName = "Battle/Combat Actions/Effect Combat Action", order = 76)]
    public class CombatAction_Effect : CombatActionBase
    {
        /// <summary>
        /// Who should the Combat Action Affect
        /// </summary>

        public BattleEffectBase effectToCast;
        public bool canEffectSelf;
        public bool canEffectTeam;
        public bool canEffectEnemy;

        public override void Cast(BattleCharacterBase caster, BattleCharacterBase target)
        {
            target.characterEffects.AddNewEffect(effectToCast);
        }
    }
}
