using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
        public class VSlice_GameManager : MonoBehaviour
        {
                public VSlice_BattleCharacterBase[] playerTeam;
                public VSlice_BattleCharacterBase[] enemyTeam;

                private List<VSlice_BattleCharacterBase> allCharacterBases = new List<VSlice_BattleCharacterBase>();

                [Header("Components")]
                public Transform[] playerTeamSpawns;
                public Transform[] enemyTeamSpawns;

                [Header("Data")]
                public VSlice_PlayerPersistentData playerPersistentData;
                public VSlice_CharacterSet defaultEnemySet;


                public static VSlice_GameManager instance;

                private void Awake()
                {
                        if (instance != null && instance != this)
                        {
                                Destroy(gameObject);
                        }
                        else
                        {
                                instance = this;
                        }
                }

                private void Start()
                {
                        CreateCharacters(playerPersistentData, defaultEnemySet);
                        VSlice_BattleTurnManager.instance.Begin();
                }

                void CreateCharacters(VSlice_PlayerPersistentData playerData, VSlice_CharacterSet enemyTeamSet)
                {
                        playerTeam = new VSlice_BattleCharacterBase[playerData.characters.Length];
                        enemyTeam = new VSlice_BattleCharacterBase[enemyTeamSet.characters.Length];

                        int playerSpawnIndex = 0;

                        //Spawn in the player Characters
                        for (int i = 0; i < playerData.characters.Length; i++)
                        {
                                if (!playerData.characters[i].isDead)
                                {
                                        VSlice_BattleCharacterBase character = CreateCharacter(playerData.characters[i].characterPrefab, playerTeamSpawns[playerSpawnIndex]);
                                        character.curHp = playerData.characters[i].health;
                                        playerTeam[i] = character;
                                        playerSpawnIndex++;
                                }
                                else
                                {
                                        playerTeam[i] = null;
                                }
                        }

                        //Spawn the enemy Characters
                        for (int i = 0; i < enemyTeamSet.characters.Length; i++)
                        {
                                VSlice_BattleCharacterBase character = CreateCharacter(enemyTeamSet.characters[i], enemyTeamSpawns[i]);
                                enemyTeam[i] = character;
                        }

                        allCharacterBases.AddRange(playerTeam);
                        allCharacterBases.AddRange(enemyTeam);
                }

                VSlice_BattleCharacterBase CreateCharacter(GameObject characterPrefab, Transform spawnPos)
                {
                        GameObject obj = Instantiate(characterPrefab, spawnPos.position, spawnPos.rotation);
                        return obj.GetComponent<VSlice_BattleCharacterBase>();
                }
        }
}