using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Arcy.Quests
{
	[System.Serializable]
	public class QuestRequirement
	{
		public RequirementEnum requirementType;
		public int requiredLvl = 0;
		public string requiredTeamMember = "";
		public QuestSO quest;
		public int requiredPreviousQuestID = 0;
		public Inventory.InventoryItem requiredItem;
		public int itemID = 0;
		public int requiredItemAmount = 1;
		public int requiredDialogueID = 0;
		public int battleID = 0;
	}

	public enum RequirementEnum
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

		// Player Level
		private SerializedProperty _requiredPlayerLvl; // int

		// Team Members
		private SerializedProperty _requiredTeamMember; // string

		// Previous Quests
		private SerializedProperty _requiredPreviousQuest; // Quest

		//Items
		private SerializedProperty _requiredItem; // item
		private SerializedProperty _itemID; // int
		private SerializedProperty _requiredItemAmount; // int	

		// Dialogue
		private SerializedProperty _dialogueID; // int

		// BattleID
		private SerializedProperty _battleID; // int

		// How to draw on the inspector window
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			_requirement = property.FindPropertyRelative("requirementType");
			// _requirementIsFullfilled = property.FindPropertyRelative("requirementIsMet");

			EditorGUILayout.PropertyField(_requirement);
			EditorGUI.indentLevel = 1;

			switch (_requirement.intValue)
			{
				case 0:
					_requiredPlayerLvl = property.FindPropertyRelative("requiredLvl");
					EditorGUILayout.PropertyField(_requiredPlayerLvl);
					break;

				case 1:
					_requiredTeamMember = property.FindPropertyRelative("requiredTeamMember");
					EditorGUILayout.PropertyField(_requiredTeamMember);
					break;

				case 2:
					_requiredPreviousQuest = property.FindPropertyRelative("quest");

					EditorGUILayout.PropertyField(_requiredPreviousQuest);
					QuestSO requiredQuest = (QuestSO)_requiredPreviousQuest.objectReferenceValue;

					if (requiredQuest != null) EditorGUILayout.LabelField("GUID", requiredQuest.questGuid.ToString());
					break;

				case 3:
					_itemID = property.FindPropertyRelative("itemID");
					_requiredItem = property.FindPropertyRelative("requiredItem");
					_requiredItemAmount = property.FindPropertyRelative("requiredItemAmount");

					EditorGUILayout.PropertyField(_requiredItem);
					Inventory.InventoryItem item = (Inventory.InventoryItem)_requiredItem.objectReferenceValue; ;

					if (item != null)
					{
						EditorGUILayout.PropertyField(_requiredItemAmount);
						EditorGUI.indentLevel++;
						EditorGUILayout.LabelField("Guid", item.GetGuid().ToString());
						EditorGUI.indentLevel--;
					}
					break;

				case 4:
					_dialogueID = property.FindPropertyRelative("requiredDialogueID");
					EditorGUILayout.PropertyField(_dialogueID);
					break;
				case 5:
					_battleID = property.FindPropertyRelative("battleID");
					EditorGUILayout.PropertyField(_battleID);
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

		// private void DrawIntProperty(SerializedProperty property, SerializedProperty requirementFullfilled)
		// {
		// 	EditorGUILayout.PropertyField(property);
		// 	EditorGUILayout.PropertyField(requirementFullfilled);
		// }

	}
#endif

}
