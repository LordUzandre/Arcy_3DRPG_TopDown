using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    public class BattleProjectile : MonoBehaviour
    {
        /// <summary>
        /// The base script for any projectile Combat Aciton. Can both damage and heal
        /// </summary>

        // Private:

        // Variables Needs to be set manually
        [SerializeField] private int _damage;
        [SerializeField] private int _heal;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private VSlice_BattleEffectBase _effectToApply;

        private BattleCharacterBase _target; //Sets automatically by CombatActionRanged
        private Vector3 _yOffset = new Vector3(0, 0.6f, 0);

        // Called by CombatActionRanged
        public void Initialize(BattleCharacterBase targetChar)
        {
            _target = targetChar;
        }

        private void Update()
        {
            // Push the projectile forward
            if (_target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.transform.position + _yOffset, _moveSpeed * Time.deltaTime);
            }
        }

        // When projectile hits its intended target
        private void OnTriggerEnter(Collider collider)
        {
            if (_target != null && collider.gameObject == _target.gameObject)
            {
                ImpactTarget();
                Destroy(gameObject);
            }
        }

        // When we hit our target.
        private void ImpactTarget()
        {
            if (_damage > 0)
                _target.TakeDamage(_damage);

            if (_heal > 0)
                _target.Heal(_heal);

            if (_effectToApply != null)
                _target.GetComponent<BattleChar_Effects>().AddNewEffect(_effectToApply);
        }
    }
}
