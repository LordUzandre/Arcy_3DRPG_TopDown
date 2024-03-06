using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Heal Combat Action", menuName = "Battle/Combat Actions/Heal Combat Action", order = 140)]
    public class CombatAction_Heal : CombatActionBase
    {
        public int healAmount;

        public override void Cast(BattleCharacterBase caster, BattleCharacterBase target)
        {
            target.Heal(healAmount);
        }
    }
}
