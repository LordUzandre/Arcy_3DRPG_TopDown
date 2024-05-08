using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Arcy.Quests
{
	[CreateAssetMenu(fileName = "new Quest", menuName = "Arcy/Quest/new QuestSO")]
	public class QuestSO : ScriptableObject
	{
		[HideInInspector] public string questName;
		[HideInInspector] public int questGuid = 0;
		[Header("UI")]
		[SerializeField] public string questGiver;

		// Requirements
		[Space]
		[SerializeField] public QuestRequirement[] requirementsToStartQuest;
		[Space]
		[SerializeField] public QuestObjective[] objectives;
		[Space]
		[Header("Rewards")]
		[SerializeField] public int goldReward;
		[SerializeField] public int experienceReward;
		[field: SerializeField] public Inventory.InventorySlot[] itemRewards;

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (questGuid == 0) questGuid = Utils.GuidGenerator.guid(this);
			if (questName != name) questName = name;
		}
#endif

	}

#if UNITY_EDITOR
	[CustomEditor(typeof(QuestSO))]
	public class QuestSOEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			QuestSO quest = (QuestSO)target;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Item Name", quest.questName.ToString()); //, EditorStyles.whiteLabel, GUILayout.ExpandHeight(true));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Guid", quest.questGuid.ToString()); //, EditorStyles.whiteLabel, GUILayout.ExpandHeight(true));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();

			base.DrawDefaultInspector();

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.HelpBox("These ScriptableObjects should contain the 'read-only'-information of any quest.", MessageType.Info);
			EditorGUILayout.EndHorizontal();
		}
	}
#endif

}
