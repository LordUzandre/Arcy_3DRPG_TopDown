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
        /// <summary>
        /// This class is the base for all characters during battle
        /// </summary> 

        public enum Team { Player, Enemy }
        public static UnityAction<VSlice_BattleCharacterBase> onCharacterDeath; // Subscribed by GameManager

        [Header("Stats")]
        public Team team; // Which side is the character on
        public string displayName; // Used by CombatActionBtn
        public int curHp; // Used by managers to update UI
        public int maxHp; // Used by managers to Update UI

        [Header("Combat Actions")]
        public VSlice_CombatAction[] combatActions; // Used by managers

        [Header("Components")]
        public VSlice_BattleCharEffects characterEffects; // Used by combatActionEffects
        public VSlice_BattleCharUI characterUI; // The character's battle UI
        public GameObject selectionVisual; // Should be a part of the prefab

        [Header("Prefabs")]
        [SerializeField] private GameObject _healParticlePrefab;

        //Private:
        private vSlice_DamageFlash _damageFlash; // Set
        private Vector3 _ogStandingPosition; // The return position after combatActionMelee

        private void Start()
        {
            _ogStandingPosition = transform.position;
            characterUI?.SetCharacterNameText(displayName);
            characterUI?.UpdateHealthBar(curHp, maxHp);
            _damageFlash = GetComponentInChildren<vSlice_DamageFlash>();
        }

        private void OnEnable()
        {
            VSlice_BattleTurnManager.instance.onNewTurn += OnNewTurn;
        }

        private void OnDisable()
        {
            VSlice_BattleTurnManager.instance.onNewTurn -= OnNewTurn;
        }

        // Called whenever BattleTurnManager trigger a new turn
        private void OnNewTurn()
        {
            // TODO: Remember to set up a character UI
            characterUI?.ToggleTurnVisual(VSlice_BattleTurnManager.instance.GetCurrentTurnCharacter() == this);
            characterEffects?.ApplyCurrentEffects();
        }

        // Makes the character cast the requested combatAction, called by combatManagers
        public void CastCombatAction(VSlice_CombatAction combatAction, VSlice_BattleCharacterBase target = null)
        {
            if (target == null)
                target = this;

            combatAction.Cast(this, target);
        }

        // Called when the character takes damage by either CombatAction or battleCharEffect
        public void TakeDamage(int damage)
        {
            curHp -= damage;

            characterUI?.UpdateHealthBar(curHp, maxHp);
            _damageFlash.Flash();

            if (curHp <= 0)
            {
                Die();
            }
        }

        // Called when the character is healed by either effect or projectile
        public void Heal(int amount)
        {
            curHp += amount;

            if (curHp > maxHp)
            {
                curHp = maxHp;
            }

            characterUI?.UpdateHealthBar(curHp, maxHp);
            Instantiate(_healParticlePrefab, transform);
        }

        // Called when hp reaches 0
        private void Die()
        {
            // TODO: Right now the object is simply destroyed. Make it something more impressive.
            onCharacterDeath?.Invoke(this);
            Destroy(gameObject);
        }

        // Used by CombatActionMelee
        public void MoveToTarget(VSlice_BattleCharacterBase target, UnityAction<VSlice_BattleCharacterBase> arriveCallback)
        {
            StartCoroutine(MeleeAttackAnimation());

            IEnumerator MeleeAttackAnimation()
            {
                // Move to the target
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

        // Enable or disable the mouse selection visual
        public void ToggleSelectionVisual(bool toggle)
        {
            selectionVisual?.SetActive(toggle);
        }
    }
}
