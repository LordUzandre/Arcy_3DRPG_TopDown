using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arcy.Battle
{
	public class VSlice_EnemyCombatManager : MonoBehaviour
	{
		[Header("AI")]
		public float minWaitTime = 0.2f;
		public float maxWaitTime = 0.5f;

		[Header("Attacking")]
		public float attackWeakestChance = 0.7f;

		[Header("Chance Curves")]
		public AnimationCurve healChanceCurve;

		private VSlice_BattleCharacterBase _curEnemy;

		private void OnEnable()
		{
			VSlice_BattleTurnManager.instance.onNewTurn += OnNewTurn;
		}

		private void OnDisable()
		{
			VSlice_BattleTurnManager.instance.onNewTurn -= OnNewTurn;
		}

		// Called when a new turn is triggered.
		private void OnNewTurn()
		{
			// Is it an enemy character's turn?
			if (VSlice_BattleTurnManager.instance.GetCurrentTurnCharacter().team == VSlice_BattleCharacterBase.Team.Enemy)
			{
				_curEnemy = VSlice_BattleTurnManager.instance.GetCurrentTurnCharacter();
				Invoke(nameof(DecideCombatAction), Random.Range(minWaitTime, maxWaitTime));
			}
		}

		// Decide which combat action to cast.
		private void DecideCombatAction()
		{
			// Do we need to to heal ourselves or a teammate?
			if (HasCombatActionOfType(typeof(VSlice_CombatActionHeal)))
			{
				VSlice_BattleCharacterBase weakestEnemy = GetWeakestCharacter(VSlice_BattleCharacterBase.Team.Enemy);

				if (Random.value < healChanceCurve.Evaluate(GetHealthPercentage(weakestEnemy)))
				{
					CastCombatAction(GetHealCombatAction(), weakestEnemy);
					return;
				}
			}

			// Deal damage to a player character
			VSlice_BattleCharacterBase playerToDamage;

			if (Random.value < attackWeakestChance)
				playerToDamage = GetWeakestCharacter(VSlice_BattleCharacterBase.Team.Player);
			else
				playerToDamage = GetRandomCharacter(VSlice_BattleCharacterBase.Team.Player);

			if (playerToDamage != null)
			{
				if (HasCombatActionOfType(typeof(VSlice_CombatActionMelee)) || HasCombatActionOfType(typeof(VSlice_CombatActionRanged)))
				{
					CastCombatAction(GetDamageCombatAction(), playerToDamage);
					return;
				}
			}

			Invoke(nameof(EndTurn), Random.Range(minWaitTime, maxWaitTime));
		}

		// Casts the requested combat action upon the requested target.
		private void CastCombatAction(VSlice_CombatAction combatAction, VSlice_BattleCharacterBase target)
		{
			if (_curEnemy == null)
			{
				EndTurn();
				return;
			}

			_curEnemy.CastCombatAction(combatAction, target);
			Invoke(nameof(EndTurn), Random.Range(minWaitTime, maxWaitTime));
		}

		// Called once the enemy has finished with their turn.
		private void EndTurn()
		{
			VSlice_BattleTurnManager.instance.EndTurn();
		}

		// Returns the percentage of health remaining for the requested character.
		// e.g. 15/20 hp = 0.75f
		private float GetHealthPercentage(VSlice_BattleCharacterBase character)
		{
			return (float)character.curHp / (float)character.maxHp;
		}

		// Does the enemy have a combat action of the requested type? (melee, ranged, heal, etc).
		private bool HasCombatActionOfType(Type type)
		{
			foreach (VSlice_CombatAction ca in _curEnemy.combatActions)
			{
				if (ca.GetType() == type)
				{
					return true;
				}
			}

			return false;
		}

		// Returns a random melee or ranged combat action from the enemy's combat action list.
		private VSlice_CombatAction GetDamageCombatAction()
		{
			VSlice_CombatAction[] ca = _curEnemy.combatActions.Where(x => x.GetType() == typeof(VSlice_CombatActionMelee) || x.GetType() == typeof(VSlice_CombatActionRanged)).ToArray();

			if (ca == null || ca.Length == 0)
				return null;

			return ca[Random.Range(0, ca.Length)];
		}

		// Returns a random heal combat action from the enemy's combat action list.
		private VSlice_CombatAction GetHealCombatAction()
		{
			VSlice_CombatAction[] ca = _curEnemy.combatActions.Where(x => x.GetType() == typeof(VSlice_CombatActionHeal)).ToArray();

			if (ca == null || ca.Length == 0)
				return null;

			return ca[Random.Range(0, ca.Length)];
		}

		// Returns a random effect combat action from the enemy's combat action list.
		private VSlice_CombatAction GetEffectCombatAction()
		{
			VSlice_CombatAction[] ca = _curEnemy.combatActions.Where(x => x.GetType() == typeof(VSlice_CombatActionEffect)).ToArray();

			if (ca == null || ca.Length == 0)
				return null;

			return ca[Random.Range(0, ca.Length)];
		}

		// Returns the weakest character from the requested team (lowest health).
		VSlice_BattleCharacterBase GetWeakestCharacter(VSlice_BattleCharacterBase.Team team)
		{
			int weakestHp = 9999;
			int weakestIndex = 0;

			VSlice_BattleCharacterBase[] characters = team == VSlice_BattleCharacterBase.Team.Player
			? VSlice_GameManager.instance.playerTeam.ToArray()
			: VSlice_GameManager.instance.enemyTeam;

			for (int i = 0; i < characters.Length; i++)
			{
				if (characters[i] == null)
					continue;

				if (characters[i].curHp < weakestHp)
				{
					weakestHp = characters[i].curHp;
					weakestIndex = i;
				}
			}

			return characters[weakestIndex];
		}

		// Returns a random character from the requested team.
		VSlice_BattleCharacterBase GetRandomCharacter(VSlice_BattleCharacterBase.Team team)
		{
			VSlice_BattleCharacterBase[] characters = null;

			if (team == VSlice_BattleCharacterBase.Team.Player)
				characters = VSlice_GameManager.instance.playerTeam.Where(x => x != null).ToArray();
			else if (team == VSlice_BattleCharacterBase.Team.Enemy)
				characters = VSlice_GameManager.instance.enemyTeam.Where(x => x != null).ToArray();

			return characters[Random.Range(0, characters.Length)];
		}
	}
}