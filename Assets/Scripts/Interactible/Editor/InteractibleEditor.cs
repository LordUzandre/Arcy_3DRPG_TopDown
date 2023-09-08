using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interactible))]
public class InteractibleEditor : Editor
{
    Object block;

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
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 100;
            SerializedProperty dialogue = serializedObject.FindProperty("dialogue");
            EditorGUILayout.PropertyField(dialogue, true);
            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel = 3;
            EditorGUILayout.BeginHorizontal();
            block = EditorGUILayout.ObjectField("Eyeheight", block, typeof(Transform), true);
            EditorGUILayout.EndHorizontal();
        }
    }
}
