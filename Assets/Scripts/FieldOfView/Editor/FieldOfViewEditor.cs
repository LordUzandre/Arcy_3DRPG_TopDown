using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Arcy.Interaction;

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

        //Draw line to current interactible
        if (fow.multipleTargetsInView)
        {
            foreach (InteractibleBase i in fow.myVisibleTargetsList)
            {
                if (i == fow.currentInteractibleBase)
                {
                    Handles.color = Color.red;
                }
                else
                {
                    Handles.color = Color.white;
                }
                Handles.DrawLine(fow.transform.position, fow.currentInteractibleBase.transform.position);
            }
        }
        else if (fow.currentInteractibleBase != null)
        {
            Handles.color = Color.red;
            Handles.DrawLine(fow.transform.position, fow.currentInteractibleBase.transform.position);
        }
    }
}
