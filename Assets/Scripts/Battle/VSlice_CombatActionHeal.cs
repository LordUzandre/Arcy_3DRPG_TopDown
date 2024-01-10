using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Heal Combat Action", menuName = "Combat Actions/Heal Combat Action")]
    public class VSlice_CombatActionHeal : VSlice_CombatAction
    {
        public int healAmount;

        public override void Cast(VSlice_BattleCharacterBase caster, VSlice_BattleCharacterBase target)
        {
            target.Heal(healAmount);
        }
    }
}
