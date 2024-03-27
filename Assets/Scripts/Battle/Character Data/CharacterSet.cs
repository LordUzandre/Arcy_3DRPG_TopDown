using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "New Enemy Set", menuName = "Arcy/Battle Character Sets/Enemy Character Set", order = 79)]
    public class CharacterSet : ScriptableObject
    {
        /// <summary>
        /// This is the class that decides the character setup for an enemy team
        /// </summary>

        public GameObject[] characters;
    }
}
