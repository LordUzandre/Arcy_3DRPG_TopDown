using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arcy.Battle
{
	public class EnemyCombatManager : MonoBehaviour
	{
		/// <summary>
		/// This class handles the enemy team's side of the combat. Should be sitting on the BattleManager-object.
		/// </summary>

		[Header("Attacking")]
		[SerializeField] private float _attackWeakestChance = 0.7f;

		[Header("Chance Curves")]
		public AnimationCurve healChanceCurve;

		private BattleCharacterBase _curEnemy;

		private float _minWaitTime = 0.2f;
		private float _maxWaitTime = 0.5f;

		private void OnEnable()
		{
			BattleTurnManager.onNewTurn += OnNewTurn;
		}

		private void OnDisable()
		{
			BattleTurnManager.onNewTurn -= OnNewTurn;
		}

		// Called by BattleTurnManager when a new turn is triggered.
		private void OnNewTurn(TurnState turnState)
		{
			switch (turnState)
			{
				// Is it an enemy character's turn?
				case (TurnState.enemyTeamsTurn):
					_curEnemy = BattleManager.instance.battleTurnManager.GetCurrentTurnCharacter();
					Invoke(nameof(DecideCombatAction), Random.Range(_minWaitTime, _maxWaitTime));
					return;
			}
		}

		// Decide which combat action to cast.
		private void DecideCombatAction()
		{
			// Do we need to to heal ourselves or a teammate?
			if (HasCombatActionOfType(typeof(CombatAction_Heal)))
			{
				BattleCharacterBase weakestEnemy = GetWeakestCharacter(BattleCharacterBase.Team.Enemy);

				if (Random.value < healChanceCurve.Evaluate(GetHealthPercentage(weakestEnemy)))
				{
					CastCombatAction(GetHealCombatAction(), weakestEnemy);
					return;
				}
			}

			// Deal damage to a player character
			BattleCharacterBase playerToDamage;

			if (Random.value < _attackWeakestChance)
				playerToDamage = GetWeakestCharacter(BattleCharacterBase.Team.Player);
			else
				playerToDamage = GetRandomCharacter(BattleCharacterBase.Team.Player);

			if (playerToDamage != null)
			{
				if (HasCombatActionOfType(typeof(CombatAction_Melee)) || HasCombatActionOfType(typeof(CombatAction_Ranged)))
				{
					CastCombatAction(GetDamageCombatAction(), playerToDamage);
					return;
				}
			}

			Invoke(nameof(EndTurn), Random.Range(_minWaitTime, _maxWaitTime));
		}

		// Casts the requested combat action upon the requested target.
		private void CastCombatAction(CombatActionBase combatAction, BattleCharacterBase target)
		{
			if (_curEnemy == null)
			{
				EndTurn();
				return;
			}

			_curEnemy.CastCombatAction(combatAction, target);
			Invoke(nameof(EndTurn), Random.Range(_minWaitTime, _maxWaitTime));
		}

		// Called once the enemy has finished with their turn.
		private void EndTurn()
		{
			BattleManager.instance.battleTurnManager.EndTurn();
		}

		// Returns the percentage of health remaining for the requested character.
		// e.g. 15/20 hp = 0.75f
		private float GetHealthPercentage(BattleCharacterBase character)
		{
			return (float)character.curHp / (float)character.maxHp;
		}

		// Does the enemy have a combat action of the requested type? (melee, ranged, heal, etc).
		private bool HasCombatActionOfType(Type type)
		{
			foreach (CombatActionBase ca in _curEnemy.combatActions)
			{
				if (ca.GetType() == type)
				{
					return true;
				}
			}

			return false;
		}

		// Returns a random melee or ranged combat action from the enemy's combat action list.
		private CombatActionBase GetDamageCombatAction()
		{
			CombatActionBase[] ca = _curEnemy.combatActions.Where(x => x.GetType() == typeof(CombatAction_Melee) || x.GetType() == typeof(CombatAction_Ranged)).ToArray();

			if (ca == null || ca.Length == 0)
				return null;

			return ca[Random.Range(0, ca.Length)];
		}

		// Returns a random heal combat action from the enemy's combat action list.
		private CombatActionBase GetHealCombatAction()
		{
			CombatActionBase[] ca = _curEnemy.combatActions.Where(x => x.GetType() == typeof(CombatAction_Heal)).ToArray();

			if (ca == null || ca.Length == 0)
				return null;

			return ca[Random.Range(0, ca.Length)];
		}

		// Returns a random effect combat action from the enemy's combat action list.
		private CombatActionBase GetEffectCombatAction()
		{
			CombatActionBase[] ca = _curEnemy.combatActions.Where(x => x.GetType() == typeof(CombatAction_Effect)).ToArray();

			if (ca == null || ca.Length == 0)
				return null;

			return ca[Random.Range(0, ca.Length)];
		}

		// Returns the weakest character from the requested team (lowest health).
		BattleCharacterBase GetWeakestCharacter(BattleCharacterBase.Team team)
		{
			int weakestHp = 9999;
			int weakestIndex = 0;

			BattleCharacterBase[] characters = team == BattleCharacterBase.Team.Player
			? BattleManager.instance.playerTeam.ToArray()
			: BattleManager.instance.enemyTeam.ToArray();

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
		BattleCharacterBase GetRandomCharacter(BattleCharacterBase.Team team)
		{
			BattleCharacterBase[] characters = null;

			if (team == BattleCharacterBase.Team.Player)
				characters = BattleManager.instance.playerTeam.Where(x => x != null).ToArray();
			else if (team == BattleCharacterBase.Team.Enemy)
				characters = BattleManager.instance.enemyTeam.Where(x => x != null).ToArray();

			return characters[Random.Range(0, characters.Length)];
		}
	}
}