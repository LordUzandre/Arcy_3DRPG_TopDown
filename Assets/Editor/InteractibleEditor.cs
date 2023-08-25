using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interactible))]
public class InteractibleEditor : Editor
{
    private bool hasDialogue = false;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        Interactible interactible = (Interactible)target;

        // bool HasDialogue
        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 100;
        hasDialogue = EditorGUILayout.Toggle("Has Dialogue", hasDialogue);
        EditorGUILayout.EndHorizontal();

        if (hasDialogue)
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
