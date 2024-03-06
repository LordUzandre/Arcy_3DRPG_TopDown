using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Arcy.Saving
{
	public class PersistentDataManager : MonoBehaviour
	{
		/// <summary>
		/// This class manages all the saving in the saving-system.
		/// </summary>

		[Header("File Storage Config")]
		[SerializeField] private string fileName;
		[SerializeField] private bool _useEncryption;

		public static PersistentDataManager instance { get; private set; }

		private GameData _gameData;
		private List<ISaveable> _persistentDataObjects;
		private FileDataHandler _dataHandler;

		private void Awake()
		{
			if (instance != null)
			{
				Debug.LogError("Found more than one Persistent Data Manager in the scene");
			}

			instance = this;
		}

		private void Start()
		{
			_dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, _useEncryption);
			_persistentDataObjects = FindAllPersistentDataObjects();
			LoadGame();
		}

		public void NewGame()
		{
			_gameData = new GameData();
		}

		public void LoadGame()
		{
			// Load any saved data from a file using the data Handler
			_gameData = _dataHandler.Load();

			// if no data can be loaded, initialize to a new game.
			if (_gameData == null)
			{
				Debug.LogError("No data was found. Initializing data defaults.");
				NewGame();
			}

			// push the loaded data to all other scripts that need it.
			foreach (ISaveable persistantDataObj in _persistentDataObjects)
			{
				persistantDataObj.LoadData(_gameData);
			}

			Debug.Log("PersistentDataManager: Loaded Data");
		}

		public void SaveGame()
		{
			// Save that data to a file using the data handler.
			foreach (ISaveable persistantDataObj in _persistentDataObjects)
			{
				persistantDataObj.SaveData(_gameData);
			}

			// save data to a file using the data handler.
			_dataHandler.Save(_gameData);
		}

		private void OnApplicationQuit()
		{
			SaveGame();
		}

		private List<ISaveable> FindAllPersistentDataObjects()
		{
			IEnumerable<ISaveable> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();

			return new List<ISaveable>(dataPersistenceObjects);
		}
	}

	public class Placeholder
	{
		// Import the following methods when you want to Open/Load game data.

		public void OnNewGameClicked()
		{
			PersistentDataManager.instance.NewGame();
		}

		public void OnLoadGameClicked()
		{
			PersistentDataManager.instance.LoadGame();
		}

		public void OnSaveGameCLicked()
		{
			PersistentDataManager.instance.SaveGame();
		}
	}
}
