using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string nameOfSpeaker;

    //Maximum number of lines
    [TextArea(1, 8)]
    public string[] sentences;
}
