using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Character Set",menuName = "Character Sets/New Character Set")]
    public class VSlice_CharacterSet : ScriptableObject
    {
    	public GameObject[] characters;
    }
}
