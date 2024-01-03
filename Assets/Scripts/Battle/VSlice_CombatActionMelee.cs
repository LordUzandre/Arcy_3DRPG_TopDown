using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Melee Combat Action", menuName = "Combat Actions/Melee Combat Action")]
    public class VSlice_CombatActionMelee : VSlice_CombatAction
    {
        public int meleeDamage;

        public override void Cast(VSlice_BattleCharacterBase caster, VSlice_BattleCharacterBase target)
        {

        }
    }
}
