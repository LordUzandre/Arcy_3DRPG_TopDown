using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Effect Combat Action", menuName = "Arcy/Battle/Combat Actions/Effect Combat Action SO")]
    public class CombatActionEffectSO : CombatActionBase
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
