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
        interactible.haveDialogue = EditorGUILayout.Toggle("Have Dialogue", interactible.haveDialogue);
        EditorGUILayout.EndHorizontal();

        if (interactible.haveDialogue)
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
