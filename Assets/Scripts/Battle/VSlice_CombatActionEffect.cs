using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Effect Combat Action", menuName = "Combat Actions/Effect Combat Action")]
    public class VSlice_CombatActionEffect : VSlice_CombatAction
    {
        public bool canEffectSelf;
        public bool canEffectTeam;
        public bool canEffectEnemy;

        public override void Cast(VSlice_BattleCharacterBase caster, VSlice_BattleCharacterBase target)
        {

        }
    }
}
