using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    public abstract class CombatActionBase : ScriptableObject
    {
        /// <summary>
        /// This is the base class for all Combat Actions during Battle.
        /// </summary>

        public string displayName;
        public string description;

        public abstract void Cast(BattleCharacterBase caster, BattleCharacterBase target);
    }
}
