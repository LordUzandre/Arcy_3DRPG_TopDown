using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    public abstract class VSlice_CombatAction : ScriptableObject
    {
        public string displayName;
        public string description;

        public abstract void Cast(VSlice_BattleCharacterBase caster, VSlice_BattleCharacterBase target);
    }
}
