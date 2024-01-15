using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [System.Serializable]
    public class PlayerPersistentCharacter
    {
        /// <summary>
        /// This class should contain all needed data for a team player character.
        /// </summary>

        public GameObject characterPrefab;
        public int health;
        public bool isDead;
    }
}
