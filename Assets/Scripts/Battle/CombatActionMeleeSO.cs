using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Melee Combat Action", menuName = "Arcy/Battle/Combat Actions/Melee Combat Action SO")]
    public class CombatActionMeleeSO : CombatActionBase
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
