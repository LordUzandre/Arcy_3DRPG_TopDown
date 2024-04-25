using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Arcy.UI
{
	[System.Serializable]
	public class InEditorEnum
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
	[CustomPropertyDrawer(typeof(InEditorEnum))]
	public class GUIEditor : PropertyDrawer
	{
		private SerializedProperty _requirement; // enum
		private SerializedProperty _requiredPlayerLvl; // int
		private SerializedProperty _requiredTeamMember; // string
		private SerializedProperty _requiredPreviousQuestID; // int
		private SerializedProperty _itemID; // int
		private SerializedProperty _dialogueID;
		private SerializedProperty _battleID; // int
		private SerializedProperty _requirementIsMet; // bool

		// How to draw on the inspector window
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			// Rect foldOutBox = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
			// property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);

			EditorGUIUtility.labelWidth = 0;
			float xStartPos = position.min.x;
			float yStartPos = position.min.y;
			float width = position.size.x * 0.99f;
			Rect drawArea = new Rect(xStartPos, yStartPos, width, EditorGUIUtility.singleLineHeight);

			_requirement = property.FindPropertyRelative("requirements");
			_requirementIsMet = property.FindPropertyRelative("requirementIsMet");

			EditorGUI.PropertyField(drawArea, _requirement, new GUIContent("Requirement type"));

			switch (_requirement.intValue)
			{
				case 0:
					_requiredPlayerLvl = property.FindPropertyRelative("requiredLvl");
					DrawPlayerLvlProperty(position, _requiredPlayerLvl, "Required Player Lvl", _requirementIsMet);
					break;
				case 1:
					_requiredTeamMember = property.FindPropertyRelative("requiredTeamMember");
					DrawPlayerLvlProperty(position, _requiredTeamMember, "Required Character in your Party", _requirementIsMet);
					break;
				case 2:
					_requiredPreviousQuestID = property.FindPropertyRelative("requiredPreviousQuestID");
					DrawPlayerLvlProperty(position, _requiredPreviousQuestID, "Required Previous Quest", _requirementIsMet);
					break;
				case 3:
					_itemID = property.FindPropertyRelative("itemID");
					DrawPlayerLvlProperty(position, _itemID, "Required Item ID", _requirementIsMet);
					break;
				case 4:
					_dialogueID = property.FindPropertyRelative("requiredDialogueID");
					DrawPlayerLvlProperty(position, _dialogueID, "Required Dialogue ID", _requirementIsMet);
					break;
				case 5:
					_battleID = property.FindPropertyRelative("battleID");
					DrawPlayerLvlProperty(position, _battleID, "Required Item ID", _requirementIsMet);
					break;
				default:
					break;
			}

			EditorGUI.EndProperty();
		}

		// request Vertical space, return it
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			int totalLines = 3;

			return (EditorGUIUtility.singleLineHeight * totalLines) + 4;
		}

		private void DrawPlayerLvlProperty(Rect position, SerializedProperty property, string title, SerializedProperty requirementFullfilled)
		{
			EditorGUIUtility.labelWidth = 0; // 0 = standard width
			EditorGUI.PropertyField(indentRect(position, 1), property, new GUIContent(title));
			EditorGUI.PropertyField(indentRect(position, 2), requirementFullfilled, new GUIContent("Requirement is fullfilled"));
		}

		private Rect indentRect(Rect pos, int numberInList)
		{
			float xStartPos = pos.min.x + 20;
			float yStartPos = pos.min.y + (EditorGUIUtility.singleLineHeight * numberInList) + (2 * numberInList);
			float width = pos.size.x * 0.99f - 20;
			float height = EditorGUIUtility.singleLineHeight;

			Rect drawArea = new Rect(xStartPos, yStartPos, width, height);
			return drawArea;
		}
	}
#endif

}
