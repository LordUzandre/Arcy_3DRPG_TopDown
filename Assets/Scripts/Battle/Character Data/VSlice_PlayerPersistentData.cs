using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    [CreateAssetMenu(fileName = "Player Persitent Data", menuName = "Character Sets/New Player Persistent Data")]
    public class VSlice_PlayerPersistentData : ScriptableObject
    {
        public VSlice_PlayerPersistentCharacter[] characters;

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
                    if (characters[i].characterPrefab.TryGetComponent<VSlice_BattleCharacterBase>(out var character))
                    {
                        characters[i].health = character.curHp;
                        characters[i].isDead = false;
                    }
                }
            }
        }
    }
}