using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Heal Combat Action", menuName = "Arcy/Battle/Combat Actions/Heal Combat Action SO")]
    public class CombatActionHealSO : CombatActionBase
    {
        public int healAmount;

        public override void Cast(BattleCharacterBase caster, BattleCharacterBase target)
        {
            target.Heal(healAmount);
        }
    }
}
