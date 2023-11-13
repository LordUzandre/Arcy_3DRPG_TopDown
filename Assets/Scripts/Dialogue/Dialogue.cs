using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/New Dialogue")]
public class Dialogue : ScriptableObject
{
    //Maximum number of lines per sentence
    [TextArea(1, 4)]
    public List<string> Sentences;
}
