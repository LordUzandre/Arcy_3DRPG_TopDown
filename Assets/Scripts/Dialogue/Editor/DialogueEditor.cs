using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Arcy.Interaction;
using Arcy.Dialogue;

namespace Arcy.Interaction
{
	[CustomEditor(typeof(DialogueBlock))]
	public class DialogueEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			// base.OnInspectorGUI();

			// DialogueBlock myDialogue = (DialogueBlock)target;

			// // bool HasDialogue
			// EditorGUILayout.BeginHorizontal();
			// EditorGUIUtility.labelWidth = 100;
			// myDialogue.questRelated = EditorGUILayout.Toggle("Quest-related", myDialogue.questRelated);
			// EditorGUILayout.EndHorizontal();

			// // bool HasDialogue
			// EditorGUILayout.BeginHorizontal();
			// EditorGUIUtility.labelWidth = 100;
			// myDialogue.singleUseDialogue = EditorGUILayout.Toggle("Single Use Dialogue", myDialogue.singleUseDialogue);
			// EditorGUILayout.EndHorizontal();

			// if (myDialogue.questRelated)
			// {
			// 	//Dialogue
			// 	EditorGUI.indentLevel = 2;
			// 	EditorGUIUtility.labelWidth = 175;
			// 	SerializedProperty dialogue = serializedObject.FindProperty("dialogue");
			// 	EditorGUILayout.PropertyField(dialogue, true);
			// 	serializedObject.ApplyModifiedProperties();
			// 	EditorGUI.indentLevel = 0;
			// }
		}
	}
}
