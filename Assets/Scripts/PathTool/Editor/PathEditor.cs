using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    PathEditor path;

    public void Draw()
    {
        for (int i = 0; i < path.NumPoints; i++)
        {
            Vector2 newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, .1f, Vector2.zero, Handles.CylinderHandleCap);
        }
    }

    public void OnEnable()
    {
        creator = (PathCreator)target;
        if (creator.path = null)
        {
            creator.CreatePath();
        }
        path = creator.path;
    }
}
