using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;

        //Draw view cone
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle * .5f, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle * .5f, false);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
        //Vector3 viewAngleC = fow.DirFromAngle(-fow.viewAngle * .25f, false);
        //Vector3 viewAngleD = fow.DirFromAngle(fow.viewAngle * .25f, false);
        //Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleC * fow.viewRadius);
        //Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleD * fow.viewRadius);
        
        //Draw line to current interactible
        if (fow.currentInteractible != null)
        {
            Handles.color = Color.red;
            Handles.DrawLine(fow.transform.position, fow.currentInteractible.transform.position);
        }
    }
}
