using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Ranged Combat Action", menuName = "Battle/Combat Actions/Ranged Combat Action", order = 130)]
    public class CombatAction_Ranged : CombatActionBase
    {
        public GameObject projectilePrefab;

        public override void Cast(BattleCharacterBase caster, BattleCharacterBase target)
        {
            if (caster == null)
                return;

            Vector3 yOffset = new Vector3(0, 0.5f, 0);
            GameObject projectile = Instantiate(projectilePrefab, caster.transform.position + yOffset, Quaternion.identity);
            projectile.GetComponent<BattleProjectile>().Initialize(target);
        }
    }
}
