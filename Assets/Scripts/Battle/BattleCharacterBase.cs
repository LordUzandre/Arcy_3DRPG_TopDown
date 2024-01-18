using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;


namespace Arcy.Battle
{
    [SelectionBase]
    public class BattleCharacterBase : MonoBehaviour
    {
        /// <summary>
        /// This class is the base for all characters during battle
        /// </summary> 

        public enum Team { Player, Enemy }
        public static UnityAction<BattleCharacterBase> onCharacterDeath; // Subscribed by GameManager

        [Header("Stats")]
        public Team team; // Which side is the character on
        public string displayName; // Used by CombatActionBtn
        public int curHp; // Used by managers to update UI
        public int maxHp; // Used by managers to Update UI

        [Header("Combat Actions")]
        public CombatActionBase[] combatActions; // Used by managers

        [Header("Components")]
        public BattleChar_Effects characterEffects; // Used by combatActionEffects
        public GameObject selectionVisual; // Visual indicator which character can be chosen for attack. Remember: should be part of prefab.
        [HideInInspector] public BattleCharUI characterUI; // The character's battle UI, set by GameManager

        [Header("Prefabs")]
        [SerializeField] private GameObject _healParticlePrefab;

        //Private:
        private DamageFlash _damageFlash; // Set
        private Vector3 _ogStandingPosition; // The return position after combatActionMelee

        private void Start()
        {
            _ogStandingPosition = transform.position;
            _damageFlash = GetComponentInChildren<DamageFlash>();
        }

        private void OnEnable()
        {
            BattleTurnManager.OnNewTurn += OnNewTurn;
        }

        private void OnDisable()
        {
            BattleTurnManager.OnNewTurn -= OnNewTurn;
        }

        // Called whenever BattleTurnManager trigger a new turn
        private void OnNewTurn(TurnState newTurnState)
        {
            characterUI?.ToggleTurnVisual(BattleManager.instance.battleTurnManager.GetCurrentTurnCharacter() == this);
            characterEffects?.ApplyCurrentEffects();
        }

        // Makes the character cast the requested combatAction, called by combatManagers
        public void CastCombatAction(CombatActionBase combatAction, BattleCharacterBase target = null)
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

            if (team == Team.Player)
                BattleManager.instance.playerTeam.Remove(this);
            else
                BattleManager.instance.enemyTeam.Remove(this);
        }

        // Used by CombatActionMelee
        public void MoveToTarget(BattleCharacterBase target, UnityAction<BattleCharacterBase> arriveCallback)
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
