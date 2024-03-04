using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Battle;

namespace Arcy.Quests
{
	[CreateAssetMenu(fileName = "Battle Requirement", menuName = "Quest/Objectives/New Battle Objective")]
	public class QuestR_Battle : QuestObjectiveBase
	{
		/// <summary>
		/// This is quest-related objective that means you should have to defeat a certain character in battle in order to progress.
		/// </summary>

		[SerializeField] private BattleCharacterBase _characterToDefeat;
		[SerializeField] private bool thisObjectiveCanBeSkipped;
		public override bool ThisObjectiveCanBeSkipped { get { return thisObjectiveCanBeSkipped; } set { thisObjectiveCanBeSkipped = value; } }

		public override event Action<QuestObjectiveBase> ObjectiveFinished;

		public override void ObjectiveActivate()
		{
		}

		public override void OnFinish()
		{
		}
	}
}
