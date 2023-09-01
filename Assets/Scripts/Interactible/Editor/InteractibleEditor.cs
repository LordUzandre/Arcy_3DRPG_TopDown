using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interactible))]
public class InteractibleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        Interactible interactible = (Interactible)target;

        // bool HasDialogue
        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 100;
        interactible.isNPC = EditorGUILayout.Toggle("is NPC", interactible.isNPC);
        EditorGUILayout.EndHorizontal();

        // bool HasDialogue
        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 100;
        interactible.hasDialogue = EditorGUILayout.Toggle("Have Dialogue", interactible.hasDialogue);
        EditorGUILayout.EndHorizontal();

        if (interactible.hasDialogue)
        {
            //Dialogue
            EditorGUI.indentLevel = 2;
            EditorGUIUtility.labelWidth = 175;
            SerializedProperty dialogue = serializedObject.FindProperty("dialogue");
            EditorGUILayout.PropertyField(dialogue, true);
            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = 0;
        }
    }
}
