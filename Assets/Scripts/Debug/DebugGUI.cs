using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGUI : MonoBehaviour
{
    public static DebugGUI singletonInstance;

    private void Awake()
    {
        if (singletonInstance == null)
        {
            singletonInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnGUI()
    {
        GUIStyle devGUIstyle = new GUIStyle(GUI.skin.label);
        devGUIstyle.alignment = TextAnchor.UpperLeft;
        devGUIstyle.fontSize = 16;
        //string guiString = $"charCurrentRotation = {charCurrectRotation} \nDesiredVector = {charDesiredVector} \nQuaternion(lookforward) = {lookRotation.ToString("f2")} ";
        //GUI.Label(new Rect(20, 20, (Screen.width / 2), Screen.height), guiString, devGUIstyle);
    }
}
