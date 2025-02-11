using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arcy.Battle
{
        public class BattleManager : MonoBehaviour
        {
                public static BattleManager instance; // Should be the only singleton on BattleManager

                [Header("Player and Enemy Teams (Sets automatically)")]
                public List<BattleCharacterBase> playerTeam; // List that includes all Player Team's characters
                public List<BattleCharacterBase> enemyTeam; // List that includes all Enemy Team's characters

                //Private:
                [Header("Components (Set manually)")]
                [SerializeField] private Transform _playerTeamSpawnPositionsParent; // The transform that's going to parent all player Team spawn points
                [SerializeField] private Transform _enemyTeamSpawnPositionsParent; // The transform that's going to parent all enemy Team spawn points
                private Transform[] _playerTeamSpawns; // ALl the transform of player Team Spawn Points
                private Transform[] _enemyTeamSpawns; // ALl the transform of enemy Team Spawn Points

                [Header("Data (Set manually)")]
                [SerializeField] private PlayerPersistentData _playerPersistentData; // Player Team's Data
                [SerializeField] private CharacterSet _EnemyCharacterSet; // Enemy Team's Data

                [Header("Character UI (set manually)")]
                [SerializeField] private GameObject _playerCharacterUiParentObject; // The parent Object that should spawn Player Character's UI
                [SerializeField] private GameObject _enemyCharacterUiParentObject; // The parent Object that should spawn Enemy Character's UI
                [SerializeField] private GameObject _characterUiPrefab; // Player Character's UI Prefab

                private List<BattleCharacterBase> _allCharactersList = new List<BattleCharacterBase>(); // All character's that currently featured in the battle, used to determine when the battle's over
                private BattleCharUI[] _playerBattleCharUIArray = new BattleCharUI[3];
                private BattleCharUI[] _enemyBattleCharUIArray = new BattleCharUI[4];

                [HideInInspector] public BattleTurnManager battleTurnManager;
                [HideInInspector] public PlayerCombatManager playerCombatManager;
                [HideInInspector] public EnemyCombatManager enemyCombatManager;

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
                        CreateCharacters(_playerPersistentData, _EnemyCharacterSet);
                        StartCoroutine(Begin());

                        battleTurnManager = GetComponent<BattleTurnManager>();
                        playerCombatManager = GetComponent<PlayerCombatManager>();
                        enemyCombatManager = GetComponent<EnemyCombatManager>();

                        IEnumerator Begin()
                        {
                                //Make sure waits one frame before Battle begins.
                                yield return null;
                                battleTurnManager.Begin();
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
                        playerTeam = new List<BattleCharacterBase>();
                        enemyTeam = new List<BattleCharacterBase>();
                        _playerBattleCharUIArray = _playerCharacterUiParentObject.GetComponentsInChildren<BattleCharUI>();
                        _enemyBattleCharUIArray = _enemyCharacterUiParentObject.GetComponentsInChildren<BattleCharUI>();

                        int playerSpawnIndex = 0;

                        // Spawn in the Player Team Characters based on PersistentPlayerData
                        foreach (PlayerPersistentCharacter unit in playerData.characters)
                        {
                                if (!unit.isDead)
                                {
                                        BattleCharacterBase character = CreateCharacter(unit.characterPrefab, _playerTeamSpawns[playerSpawnIndex]);
                                        character.curHp = unit.health;

                                        character.characterUI = _playerBattleCharUIArray[playerSpawnIndex]; // TODO: Replace with a method that spawns a new UI.
                                        character.characterUI.ConnectUItoNewChar(character.displayName, character.curHp, character.maxHp);

                                        playerTeam.Add(character);
                                }
                                playerSpawnIndex++;
                        }

                        int enemySpawnIndex = 0;

                        //Spawn the enemy Characters based on enemyTeam-List
                        for (int i = 0; i < enemyTeamSet.characters.Length; i++)
                        {
                                BattleCharacterBase character = CreateCharacter(enemyTeamSet.characters[i], _enemyTeamSpawns[enemySpawnIndex]);
                                enemyTeam.Add(character);
                                enemySpawnIndex++;

                                character.characterUI = _enemyBattleCharUIArray[i]; // TODO: Replace with a method that spawns a new UI.
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

                        foreach (BattleCharacterBase _char in _allCharactersList)
                        {
                                if (_char.team == BattleCharacterBase.Team.Player)
                                        playersRemaining++;
                                else
                                        enemiesRemaining++;
                        }

                        if (enemiesRemaining == 0)
                        {
                                BattleIsOver(BattleCharacterBase.Team.Player);
                        }
                        else if (playersRemaining == 0)
                        {
                                BattleIsOver(BattleCharacterBase.Team.Enemy);
                        }
                }

                private void BattleIsOver(BattleCharacterBase.Team winningSide)
                {
                        if (winningSide == BattleCharacterBase.Team.Player) //Player won!
                        {
                                _winningTeam = "Players Team";
                                Invoke(nameof(LoadMapScene), 0.5f);

                                // Update the team's stats when winning.
                                for (int i = 0; i < playerTeam.Count; i++)
                                {
                                        if (playerTeam[i] != null)
                                                _playerPersistentData.characters[i].health = playerTeam[i].curHp;
                                        else
                                                _playerPersistentData.characters[i].isDead = true;
                                }
                        }
                        else if (winningSide == BattleCharacterBase.Team.Enemy) // Enemy won!
                        {
                                _playerPersistentData.ResetCharacters();
                                _winningTeam = "Enemy Team";
                                Invoke(nameof(LoadMapScene), 0.5f);
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