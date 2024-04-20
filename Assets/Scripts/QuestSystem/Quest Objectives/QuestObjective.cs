using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Management;
using Unity.VisualScripting;
using UnityEngine;

namespace Arcy.Quests
{
	// [CreateAssetMenu(fileName = "new Objective", menuName = "Arcy/Quests/Objectives")]
	public abstract class QuestObjective : ScriptableObject
	{
		/// <summary>
		/// This class acts as a parent class for quest objectives.
		/// Child scripts should be attached to gameobjects and attached as prefabs on the questInfoSO.
		/// The prefabs will be spawned into the scene by QuestManager and destroyed upon completion.
		/// </summary>

		[Header("Status text for Journal UI")]
		[TextArea(2, 6)][SerializeField] public string uiStatusText;

		public abstract bool objectiveCanBeSkipped { get; set; }

		public abstract void InitializeObjective();
		public abstract void FinishObjective();
	}
}
