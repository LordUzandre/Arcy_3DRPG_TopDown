using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    public class VSlice_BattleCharEffects : MonoBehaviour
    {
        private List<EffectInstance> _curEffects = new List<EffectInstance>();
        private VSlice_BattleCharacterBase _character;

        private void Start()
        {
            _character = GetComponent<VSlice_BattleCharacterBase>();
        }

        // Called by CombatActionEffect
        public void AddNewEffect(VSlice_BattleEffectBase effect)
        {
            EffectInstance effectInstance = new EffectInstance(effect);

            if (effect.activePrefab != null)
            {
                effectInstance.curActiveGameObject = Instantiate(effect.activePrefab, transform);
            }

            if (effect.tickPrefab != null)
            {
                effectInstance.curTickParticle = Instantiate(effect.tickPrefab, transform).GetComponent<ParticleSystem>();
            }

            _curEffects.Add(effectInstance);
            ApplyEffect(effectInstance);
        }

        // Called by BattleCharacterBase
        public void ApplyCurrentEffects()
        {
            for (int i = 0; i < _curEffects.Count; i++)
            {
                ApplyEffect(_curEffects[i]);
            }
        }

        private void ApplyEffect(EffectInstance effect)
        {
            effect.curTickParticle.Play();

            if (effect.effect as DamageEffect)
            {
                _character.TakeDamage((effect.effect as DamageEffect).damage);
            }
            else if (effect.effect as HealEffect)
            {
                _character.Heal((effect.effect as HealEffect).heal);
            }

            effect.turnRemaining--;

            if (effect.turnRemaining == 0)
            {
                RemoveEffect(effect);
            }
        }

        // Remove the effect once it has run its course
        private void RemoveEffect(EffectInstance effect)
        {
            if (effect.curActiveGameObject != null)
                Destroy(effect.curActiveGameObject);

            if (effect.curTickParticle != null)
                Destroy(effect.curTickParticle.gameObject);

            _curEffects.Remove(effect);
        }
    }
}
