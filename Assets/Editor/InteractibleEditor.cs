using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Interactible))]
public class InteractibleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Call normal GUI (displaying "a" and any other variables you might have)
        base.OnInspectorGUI();

        // Reference the variables in the script
        Interactible interactible = (Interactible)target;

        if (interactible.isNPC)
        {
            // Ensure the label and the value are on the same line
            EditorGUILayout.BeginHorizontal();

            // A label that says "b" (change b to B if you want it uppercase like default) and restrict its length.
            // You can change 50 to any other value
            EditorGUILayout.LabelField("speakerID", GUILayout.MaxWidth(50));

            // Show and save the value of b
            interactible.speakerID = EditorGUILayout.TextField(interactible.speakerID);
            // If you would like to restrict the length of the int field, replace the above line with this one:
            // script.b = EditorGUILayout.IntField(script.b, GUILayout.MaxWidth(50)); // (or any other value other than 50)

            EditorGUILayout.EndHorizontal();
        }
    }
}
