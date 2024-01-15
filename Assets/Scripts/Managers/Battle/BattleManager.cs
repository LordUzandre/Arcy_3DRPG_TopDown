using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arcy.Battle
{
        public class BattleManager : MonoBehaviour
        {
                public static BattleManager instance; //Singleton

                [Header("Player and Enemy Teams (Sets automatically)")]
                public List<BattleCharacterBase> playerTeam; // List that includes all Player Team's characters
                public BattleCharacterBase[] enemyTeam; // Array that includes all Enemy Team's characters

                //Private:
                [Header("Components (Set manually)")]
                [SerializeField] private Transform _playerTeamSpawnPositionsParent; // The transform that's going to parent all player Team spawn points
                [SerializeField] private Transform _enemyTeamSpawnPositionsParent; // The transform that's going to parent all enemy Team spawn points
                private Transform[] _playerTeamSpawns; // ALl the transform of player Team Spawn Points
                private Transform[] _enemyTeamSpawns; // ALl the transform of enemy Team Spawn Points

                [Header("Data (Set manually)")]
                [SerializeField] private PlayerPersistentData _playerPersistentData; // Player Team's Data
                [SerializeField] private CharacterSet _defaultEnemySet; // Enemy Team's Data

                [Header("Character UI (set manually)")]
                [SerializeField] private GameObject _playerCharacterUiParentObject; // The parent Object that should spawn Player Character's UI
                [SerializeField] private GameObject _enemyCharacterUiParentObject; // The parent Object that should spawn Enemy Character's UI
                [SerializeField] private GameObject _characterUiPrefab; // Player Character's UI Prefab

                private List<BattleCharacterBase> _allCharactersList = new List<BattleCharacterBase>(); // All character's that currently featured in the battle, used to determine when the battle's over
                private BattleCharUI[] _playerBattleCharUIArray = new BattleCharUI[3];
                private BattleCharUI[] _enemyBattleCharUIArray = new BattleCharUI[4];

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

                        IEnumerator Begin()
                        {
                                //Make sure waits one frame before Battle begins.
                                yield return null;
                                BattleTurnManager.instance.Begin();
                        }
                }

                // Set the player and enemy team based on persistent data
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
                        BattleCharacterBase.onCharacterDeath += OnCharacterKilled;
                }

                private void OnDisable()
                {
                        BattleCharacterBase.onCharacterDeath -= OnCharacterKilled;
                }

                // Called at the start of the game - create the character game objects
                private void CreateCharacters(PlayerPersistentData playerData, CharacterSet enemyTeamSet)
                {
                        // playerTeam = new VSlice_BattleCharacterBase[playerData.characters.Length];
                        playerTeam = new List<BattleCharacterBase>();
                        enemyTeam = new BattleCharacterBase[enemyTeamSet.characters.Length];
                        _playerBattleCharUIArray = _playerCharacterUiParentObject.GetComponentsInChildren<BattleCharUI>();
                        _enemyBattleCharUIArray = _enemyCharacterUiParentObject.GetComponentsInChildren<BattleCharUI>();

                        int playerSpawnIndex = 0;

                        // Spawn in the Player Team Characters
                        for (int i = 0; i < playerData.characters.Length; i++)
                        {
                                if (!playerData.characters[i].isDead)
                                {
                                        BattleCharacterBase character = CreateCharacter(playerData.characters[i].characterPrefab, _playerTeamSpawns[playerSpawnIndex]);
                                        character.curHp = playerData.characters[i].health;

                                        // Spawn UI and connect to newly formed player character
                                        //character.characterUI = Instantiate(_characterUiPrefab, _playerCharacterUiParentObject.transform).GetComponent<BattleCharUI>();
                                        character.characterUI = _playerBattleCharUIArray[i];
                                        character.characterUI.ConnectUItoNewChar(character.displayName, character.curHp, character.maxHp);

                                        playerTeam.Add(character);
                                        playerSpawnIndex++;
                                }
                        }

                        //Spawn the enemy Characters
                        for (int i = 0; i < enemyTeamSet.characters.Length; i++)
                        {
                                BattleCharacterBase character = CreateCharacter(enemyTeamSet.characters[i], _enemyTeamSpawns[i]);
                                enemyTeam[i] = character;
                                character.characterUI = _enemyBattleCharUIArray[i];
                                character.characterUI.ConnectUItoNewChar(character.displayName, character.curHp, character.maxHp);
                        }

                        _allCharactersList.AddRange(playerTeam);
                        _allCharactersList.AddRange(enemyTeam);
                }

                // Spawn in the characters
                private BattleCharacterBase CreateCharacter(GameObject characterPrefab, Transform spawnPos)
                {
                        GameObject obj = Instantiate(characterPrefab, spawnPos.position, spawnPos.rotation);
                        obj.transform.parent = spawnPos.transform;
                        return obj.GetComponent<BattleCharacterBase>();
                }

                // Whenever a character (good or bad) is killed
                private void OnCharacterKilled(BattleCharacterBase character)
                {
                        _allCharactersList.Remove(character);

                        int playersRemaining = 0;
                        int enemiesRemaining = 0;

                        for (int i = 0; i < _allCharactersList.Count; i++)
                        {
                                if (_allCharactersList[i].team == BattleCharacterBase.Team.Player)
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

                // Update the team's stats when winning.
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

                // Return to the main map once the battle is finished
                private void LoadMapScene()
                {
                        //SceneManager.LoadScene("Map");
                        Debug.Log($"Battle is Finished, {_winningTeam} won!");
                }
        }
}