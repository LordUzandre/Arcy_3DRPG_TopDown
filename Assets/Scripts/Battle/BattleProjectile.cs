using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    public class BattleProjectile : MonoBehaviour
    {
        public int damage;
        public int heal;
        public float moveSpeed;
        public VSlice_BattleEffectBase effectToApply;

        private Vector3 yOffset = new Vector3(0, 0.5f, 0);
        private VSlice_BattleCharacterBase target;

        public void Initialize(VSlice_BattleCharacterBase targetChar)
        {
            target = targetChar;
        }

        private void Update()
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position + yOffset, moveSpeed * Time.deltaTime);
            }
        }

        // Called when we hit our target.
        private void ImpactTarget()
        {
            if (damage > 0)
                target.TakeDamage(damage);

            if (heal > 0)
                target.Heal(heal);

            if (effectToApply != null)
                target.GetComponent<VSlice_BattleCharEffects>().AddNewEffect(effectToApply);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target != null && other.gameObject == target.gameObject)
            {
                ImpactTarget();
                Destroy(gameObject);
            }
        }
    }
}
