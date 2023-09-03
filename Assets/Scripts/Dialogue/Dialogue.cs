using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "New Dialogue")]
public class Dialogue : ScriptableObject
{
    //Maximum number of lines
    [TextArea(1, 6)]
    public List<string> sentences;
}
