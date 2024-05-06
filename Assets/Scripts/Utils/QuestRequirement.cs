using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Arcy.Quests
{
	[System.Serializable]
	public class QuestRequirement
	{
		public Requirement requirements;
		public int requiredLvl = 0;
		public string requiredTeamMember = "";
		public int requiredPreviousQuestID = 0;
		public int itemID = 0;
		public int requiredDialogueID = 0;
		public int battleID = 0;
		public bool requirementIsMet = false;
	}

	public enum Requirement
	{
		PlayerLevel = 0,
		TeamMember = 1,
		PreviousQuestID = 2,
		ItemID = 3,
		DialogueID = 4,
		BattleID = 5
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(QuestRequirement))]
	public class GUIEditor : PropertyDrawer
	{
		private SerializedProperty _requirement; // enum
		private SerializedProperty _requiredPlayerLvl; // int
		private SerializedProperty _requiredTeamMember; // string
		private SerializedProperty _requiredPreviousQuestID; // int
		private SerializedProperty _itemID; // int
		private SerializedProperty _dialogueID;
		private SerializedProperty _battleID; // int
		private SerializedProperty _requirementIsFullfilled; // bool

		// How to draw on the inspector window
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			_requirement = property.FindPropertyRelative("requirements");
			_requirementIsFullfilled = property.FindPropertyRelative("requirementIsMet");

			EditorGUILayout.PropertyField(_requirement);
			EditorGUI.indentLevel = 1;

			switch (_requirement.intValue)
			{
				case 0:
					_requiredPlayerLvl = property.FindPropertyRelative("requiredLvl");
					DrawIntProperty(_requiredPlayerLvl, _requirementIsFullfilled);
					break;
				case 1:
					_requiredTeamMember = property.FindPropertyRelative("requiredTeamMember");
					DrawIntProperty(_requiredTeamMember, _requirementIsFullfilled);
					break;
				case 2:
					_requiredPreviousQuestID = property.FindPropertyRelative("requiredPreviousQuestID");
					DrawIntProperty(_requiredPreviousQuestID, _requirementIsFullfilled);
					break;
				case 3:
					_itemID = property.FindPropertyRelative("itemID");
					DrawIntProperty(_itemID, _requirementIsFullfilled);
					break;
				case 4:
					_dialogueID = property.FindPropertyRelative("requiredDialogueID");
					DrawIntProperty(_dialogueID, _requirementIsFullfilled);
					break;
				case 5:
					_battleID = property.FindPropertyRelative("battleID");
					DrawIntProperty(_battleID, _requirementIsFullfilled);
					break;
				default:
					break;
			}

			EditorGUI.indentLevel = 0;
		}

		// public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		// {
		// 	return 0;
		// }

		private void DrawIntProperty(SerializedProperty property, SerializedProperty requirementFullfilled)
		{
			EditorGUILayout.PropertyField(property);
			EditorGUILayout.PropertyField(requirementFullfilled);
		}

	}
#endif

}
