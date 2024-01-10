using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    public class VSlice_BattleCharEffects : MonoBehaviour
    {
        private List<EffectInstance> curEffects = new List<EffectInstance>();
        private VSlice_BattleCharacterBase character;

        private void Awake()
        {
            character = GetComponent<VSlice_BattleCharacterBase>();
        }

        public void AddNewEffect(VSlice_BattleEffectBase effect)
        {
            EffectInstance effectInstance = new EffectInstance(effect);

            if (effect.activePrefab == null)
            {
                effectInstance.curActiveGameObject = Instantiate(effect.activePrefab, transform);
            }

            if (effect.tickPrefab != null)
            {
                effectInstance.curTickParticle = Instantiate(effect.tickPrefab, transform).GetComponent<ParticleSystem>();
            }

            curEffects.Add(effectInstance);
            ApplyEffect(effectInstance);
        }

        public void ApplyCurrentEffects()
        {
            for (int i = 0; i < curEffects.Count; i++)
            {
                ApplyEffect(curEffects[i]);
            }
        }

        private void ApplyEffect(EffectInstance effect)
        {
            effect.curTickParticle.Play();

            if (effect.effect as DamageEffect)
            {
                character.TakeDamage((effect.effect as DamageEffect).damage);
            }
            else if (effect.effect as HealEffect)
            {
                character.Heal((effect.effect as HealEffect).heal);
            }

            effect.turnRemaining--;

            if (effect.turnRemaining == 0)
            {
                RemoveEffect(effect);
            }
        }

        private void RemoveEffect(EffectInstance effect)
        {
            if (effect.curActiveGameObject != null)
                Destroy(effect.curActiveGameObject);

            if (effect.curTickParticle != null)
                Destroy(effect.curTickParticle.gameObject);

            curEffects.Remove(effect);
        }
    }
}
