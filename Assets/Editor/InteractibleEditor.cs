using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interactible))]
public class InteractibleEditor : Editor
{
    private bool hasDialogue = false;
    private bool patrolRoute = false;

    public override void OnInspectorGUI()
    {
        // Call normal GUI (displaying "a" and any other variables you might have)
        //base.OnInspectorGUI();

        Interactible interactible = (Interactible)target;

        hasDialogue = EditorGUILayout.Toggle("Has Dialogue", hasDialogue);

        if (hasDialogue)
        {
            // patrolRoute = EditorGUILayout.Toggle("Patrol Route", patrolRoute);

            // EditorGUILayout.BeginHorizontal();
            // EditorGUILayout.LabelField("speakerName", GUILayout.MaxWidth(125));
            // interactible.speakerID = EditorGUILayout.TextField(interactible.speakerID);
            // EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < interactible.dialogue.sentences.Length; i++)
            {
                interactible.dialogue.sentences[i] = EditorGUILayout.TextField("Element 01", interactible.dialogue.sentences[i]); ;
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
