using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Player Team Persitent Data", menuName = "Battle Character Sets/Player Team Persistent Data", order = 150)]
    public class PlayerPersistentData : ScriptableObject
    {
        /// <summary>
        /// This class is used to the character-setup in the player's party.
        /// </summary>

        public PlayerPersistentCharacter[] characters;

#if UNITY_EDITOR
        private void OnValidate()
        {
            ResetCharacters();
        }
#endif

        public void ResetCharacters()
        {
            for (int i = 0; i < characters.Length; i++)
            {
                // refactored lines to prevent MissingReferenceException error on opening project
                if (characters[i].characterPrefab != null)
                {
                    if (characters[i].characterPrefab.TryGetComponent<BattleCharacterBase>(out var character))
                    {
                        characters[i].health = character.curHp;
                        characters[i].isDead = false;
                    }
                }
            }
        }
    }
}