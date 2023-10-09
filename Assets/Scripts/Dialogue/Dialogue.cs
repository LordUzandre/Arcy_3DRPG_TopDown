using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "New Dialogue")]
public class Dialogue : ScriptableObject
{
    //Maximum number of lines per sentence
    [TextArea(1, 4)]
    public List<string> engSentences;
    [TextArea(1, 4)]
    public List<string> espSentences;
}
