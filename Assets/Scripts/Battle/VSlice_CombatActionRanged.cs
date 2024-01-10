using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Ranged Combat Action", menuName = "Combat Actions/Ranged Combat Action")]
    public class VSlice_CombatActionRanged : VSlice_CombatAction
    {
        public GameObject projectilePrefab;

        public override void Cast(VSlice_BattleCharacterBase caster, VSlice_BattleCharacterBase target)
        {
            if (caster == null)
                return;

            Vector3 yOffset = new Vector3(0, 0.5f, 0);
            GameObject projectile = Instantiate(projectilePrefab, caster.transform.position + yOffset, Quaternion.identity);
            projectile.GetComponent<BattleProjectile>().Initialize(target);
        }
    }
}
