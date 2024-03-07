using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Melee Combat Action", menuName = "Battle/Combat Actions/Melee Combat Action", order = 70)]
    public class CombatAction_Melee : CombatActionBase
    {
        public int meleeDamage;

        public override void Cast(BattleCharacterBase caster, BattleCharacterBase target)
        {
            caster.MoveToTarget(target, OnDamageTargetCallback);
        }

        private void OnDamageTargetCallback(BattleCharacterBase target)
        {
            target.TakeDamage(meleeDamage);
        }
    }
}
