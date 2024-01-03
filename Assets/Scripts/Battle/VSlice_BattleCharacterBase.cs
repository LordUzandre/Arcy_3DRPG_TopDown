using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //public DamageFlash damageFlash;

        [Header("Prefabs")]
        public GameObject healParticlePrefab;

        //Private:
        private Vector3 standingPosition;

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
            characterUI.ToggleTurnVisual(VSlice_BattleTurnManager.instance.GetCurrentCharacter() == this);
        }
    }
}
