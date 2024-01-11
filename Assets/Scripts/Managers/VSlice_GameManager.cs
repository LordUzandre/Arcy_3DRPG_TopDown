using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arcy.Battle
{
        public class VSlice_GameManager : MonoBehaviour
        {
                public static VSlice_GameManager instance; //Singleton

                [Header("Player and Enemy Teams (Sets automatically)")]
                public List<VSlice_BattleCharacterBase> playerTeam; // List that includes all Player Team's characters
                public VSlice_BattleCharacterBase[] enemyTeam; // Array that includes all Enemy Team's characters

                //Private:
                [Header("Components (Set manually)")]
                [SerializeField] private Transform _playerTeamSpawnPositionsParent; // The transform that's going to parent all player Team spawn points
                [SerializeField] private Transform _enemyTeamSpawnPositionsParent; // The transform that's going to parent all enemy Team spawn points
                private Transform[] _playerTeamSpawns; // ALl the transform of player Team Spawn Points
                private Transform[] _enemyTeamSpawns; // ALl the transform of enemy Team Spawn Points

                [Header("Data (Set manually)")]
                [SerializeField] private VSlice_PlayerPersistentData _playerPersistentData; // Player Team's Data
                [SerializeField] private VSlice_CharacterSet _defaultEnemySet; // Enemy Team's Data

                [Header("Character UI (set manually)")]
                [SerializeField] private GameObject _characterUiParentObject; // The parent Object that should spawn Player Character's UI
                [SerializeField] private GameObject _characterUiPrefab; // Player Character's UI Prefab

                private List<VSlice_BattleCharacterBase> _allCharactersList = new List<VSlice_BattleCharacterBase>(); // All character's that currently featured in the battle, used to determine when the battle's over

                // Debug
                private string _winningTeam = "null";

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
                        PopulateArrays();
                        CreateCharacters(_playerPersistentData, _defaultEnemySet);
                        StartCoroutine(Begin());
                }

                private void PopulateArrays()
                {
                        _playerTeamSpawns = new Transform[_playerTeamSpawnPositionsParent.childCount];

                        for (int i = 0; i < _playerTeamSpawnPositionsParent.childCount; i++)
                        {
                                _playerTeamSpawns[i] = _playerTeamSpawnPositionsParent.GetChild(i);
                        }

                        _enemyTeamSpawns = new Transform[_enemyTeamSpawnPositionsParent.childCount];

                        for (int i = 0; i < _enemyTeamSpawnPositionsParent.childCount; i++)
                        {
                                _enemyTeamSpawns[i] = _enemyTeamSpawnPositionsParent.GetChild(i);
                        }
                }


                private void OnEnable()
                {
                        VSlice_BattleCharacterBase.onCharacterDeath += OnCharacterKilled;
                }

                private void OnDisable()
                {
                        VSlice_BattleCharacterBase.onCharacterDeath -= OnCharacterKilled;
                }

                //Make sure waits one frame before Battle begins.
                IEnumerator Begin()
                {
                        yield return null;
                        VSlice_BattleTurnManager.instance.Begin();
                }

                // Called at the start of the game - create the character game objects
                private void CreateCharacters(VSlice_PlayerPersistentData playerData, VSlice_CharacterSet enemyTeamSet)
                {
                        // playerTeam = new VSlice_BattleCharacterBase[playerData.characters.Length];
                        playerTeam = new List<VSlice_BattleCharacterBase>();
                        enemyTeam = new VSlice_BattleCharacterBase[enemyTeamSet.characters.Length];

                        int playerSpawnIndex = 0;

                        // Spawn in the Player Team Characters
                        for (int i = 0; i < playerData.characters.Length; i++)
                        {
                                if (!playerData.characters[i].isDead)
                                {
                                        VSlice_BattleCharacterBase character = CreateCharacter(playerData.characters[i].characterPrefab, _playerTeamSpawns[playerSpawnIndex]);
                                        character.curHp = playerData.characters[i].health;

                                        // Spawn UI and connect to newly formed player character
                                        character.characterUI = Instantiate(_characterUiPrefab, _characterUiParentObject.transform).GetComponent<VSlice_BattleCharUI>();
                                        character.characterUI.ConnectUItoNewChar(character.displayName, character.curHp, character.maxHp);
                                        playerTeam.Add(character);
                                        playerSpawnIndex++;
                                }
                        }

                        //Spawn the enemy Characters
                        for (int i = 0; i < enemyTeamSet.characters.Length; i++)
                        {
                                VSlice_BattleCharacterBase character = CreateCharacter(enemyTeamSet.characters[i], _enemyTeamSpawns[i]);
                                enemyTeam[i] = character;
                        }

                        _allCharactersList.AddRange(playerTeam);
                        _allCharactersList.AddRange(enemyTeam);
                }

                VSlice_BattleCharacterBase CreateCharacter(GameObject characterPrefab, Transform spawnPos)
                {
                        GameObject obj = Instantiate(characterPrefab, spawnPos.position, spawnPos.rotation);
                        return obj.GetComponent<VSlice_BattleCharacterBase>();
                }

                void OnCharacterKilled(VSlice_BattleCharacterBase character)
                {
                        _allCharactersList.Remove(character);

                        int playersRemaining = 0;
                        int enemiesRemaining = 0;

                        for (int i = 0; i < _allCharactersList.Count; i++)
                        {
                                if (_allCharactersList[i].team == VSlice_BattleCharacterBase.Team.Player)
                                        playersRemaining++;
                                else
                                        enemiesRemaining++;
                        }

                        if (enemiesRemaining == 0)
                        {
                                PlayerTeamsWins();
                        }
                        else if (playersRemaining == 0)
                        {
                                EnemyTeamWins();
                        }

                }

                private void PlayerTeamsWins()
                {
                        UpdatePlayerPersistentData();
                        Invoke(nameof(LoadMapScene), 0.5f);
                        _winningTeam = "Players Team";
                }

                private void EnemyTeamWins()
                {
                        _playerPersistentData.ResetCharacters();
                        Invoke(nameof(LoadMapScene), 0.5f);
                        _winningTeam = "Enemy Team";
                }

                private void UpdatePlayerPersistentData()
                {
                        for (int i = 0; i < playerTeam.Count; i++)
                        {
                                if (playerTeam[i] != null)
                                {
                                        _playerPersistentData.characters[i].health = playerTeam[i].curHp;
                                }
                                else
                                {
                                        _playerPersistentData.characters[i].isDead = true;
                                }
                        }
                }

                private void LoadMapScene()
                {
                        //SceneManager.LoadScene("Map");
                        Debug.Log($"Battle is Finished, {_winningTeam} won!");
                }
        }
}