using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

namespace Arcy.Battle
{
    [SelectionBase]
    public class VSlice_BattleCharacterBase : MonoBehaviour
    {
        public enum Team
        {
            Player,
            Enemy
        }

        [Header("Stats")]
        public Team team;
        public string displayName;
        public int curHp;
        public int maxHp;

        [Header("Combat Actions")]
        public VSlice_CombatAction[] combatActions;

        [Header("Components")]
        public VSlice_BattleCharEffects characterEffects;
        public VSlice_BattleCharUI characterUI;
        public GameObject selectionVisual;
        public vSlice_DamageFlash damageFlash;

        [Header("Prefabs")]
        public GameObject healParticlePrefab;
        public static UnityAction<VSlice_BattleCharacterBase> onCharacterDeath;

        //Private:
        private Vector3 _ogStandingPosition;

        private void Start()
        {
            _ogStandingPosition = transform.position;
            characterUI?.SetCharacterNameText(displayName);
            characterUI?.UpdateHealthBar(curHp, maxHp);
        }

        private void OnEnable()
        {
            VSlice_BattleTurnManager.instance.onNewTurn += OnNewTurn;
        }

        private void OnDisable()
        {
            VSlice_BattleTurnManager.instance.onNewTurn -= OnNewTurn;
        }

        void OnNewTurn()
        {
            // TODO: Remember to set up a character UI
            characterUI?.ToggleTurnVisual(VSlice_BattleTurnManager.instance.GetCurrentTurnCharacter() == this);
            characterEffects?.ApplyCurrentEffects();
        }

        public void CastCombatAction(VSlice_CombatAction combatAction, VSlice_BattleCharacterBase target = null)
        {
            if (target == null)
                target = this;

            combatAction.Cast(this, target);
        }

        public void TakeDamage(int damage)
        {
            curHp -= damage;

            characterUI?.UpdateHealthBar(curHp, maxHp);
            damageFlash?.Flash();

            if (curHp <= 0)
            {
                Die();
            }
        }

        public void Heal(int amount)
        {
            curHp += amount;

            if (curHp > maxHp)
            {
                curHp = maxHp;
            }

            characterUI?.UpdateHealthBar(curHp, maxHp);
            Instantiate(healParticlePrefab, transform);
        }

        void Die()
        {
            onCharacterDeath?.Invoke(this);
            Destroy(gameObject);
        }

        //Used for the melee combat action
        public void MoveToTarget(VSlice_BattleCharacterBase target, UnityAction<VSlice_BattleCharacterBase> arriveCallback)
        {
            StartCoroutine(MeleeAttackAnimation());

            IEnumerator MeleeAttackAnimation()
            {
                //move to the target
                while (transform.position != target.transform.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 40 * Time.deltaTime);
                    yield return null;
                }

                // Attack the target
                arriveCallback?.Invoke(target);

                // Move back to your original standing position
                while (transform.position != _ogStandingPosition)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _ogStandingPosition, 40 * Time.deltaTime);
                    yield return null;
                }
            }
        }

        public void ToggleSelectionVisual(bool toggle)
        {
            selectionVisual?.SetActive(toggle);
        }
    }
}
