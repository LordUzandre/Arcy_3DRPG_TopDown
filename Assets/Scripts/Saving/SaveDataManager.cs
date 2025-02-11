using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using Arcy.Management;

namespace Arcy.Saving
{
	public class SaveDataManager : MonoBehaviour
	{
		/// <summary>
		/// This class manages all the saving in the saving-system.
		/// </summary>

		[Header("File Storage Config")]
		[SerializeField] private bool _globalOverrideSaveData = false;
		public static bool GlobalOverrideSaveData = false;
		[Space]
		[SerializeField] private bool _createSaveDataIfNull = true;
		[SerializeField] private bool _saveOnApplicationQuit = true;
		[SerializeField] private bool _debugging = false;
		[Space]
		[SerializeField] private string _saveDataFileName = "game.save";
		[SerializeField] private bool _useEncryption = false;

		private SaveData _saveData;
		private List<ISaveableEntity> _persistentDataObjects;
		private JsonFileDataHandler _saveDataHandler;

		// MARK: PUBLIC:

		public void CreateNewSaveFile()
		{
			_saveData = new SaveData();
		}

		public void LoadGame()
		{
			// Load any saved data from a file using the data Handler
			_saveData = _saveDataHandler.Load();

			// start a new game if the data is null and we're configured to initialize data for debugging purposes
			if (_saveData == null && _createSaveDataIfNull)
			{
				CreateNewSaveFile();
				if (_debugging) Debug.Log("New Save-file created");
			}

			// if no data can be loaded, don't continue.
			if (_saveData == null)
			{
				Debug.LogError("No data was found (and createSaveDataIfNull is turned off). \n A new game needs to be started before data can be loaded.");
				return;
			}

			// push the loaded data to all other scripts that need it.
			foreach (ISaveableEntity persistantDataObj in _persistentDataObjects)
			{
				persistantDataObj.LoadData(_saveData);
			}

			if (_debugging) Debug.Log("PersistentDataManager: Loaded Data");
		}

		public void SaveGame()
		{
			if (_saveData == null)
			{
				Debug.LogWarning("No data was found. A new game needs to be started before data can be saved");
				return;
			}

			// Change all the save-data in the json-file
			foreach (ISaveableEntity persistantDataObj in _persistentDataObjects)
			{
				persistantDataObj.SaveData(_saveData);
			}

			// write all the changes to the json-file
			_saveDataHandler.Save(_saveData);
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			_persistentDataObjects = FindAllSaveDataObjects();
			LoadGame();
		}

		private void OnSceneUnloaded(Scene scene)
		{
			SaveGame();
		}

		public bool HasGameData()
		{
			return _saveData != null;
		}

		//MARK: PRIVATE:

		private void Awake()
		{
			GlobalOverrideSaveData = _globalOverrideSaveData;
			_saveDataHandler = new JsonFileDataHandler(Application.persistentDataPath, _saveDataFileName, _useEncryption);
		}

		private void OnEnable()
		{
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
			UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
		}

		private void OnDisable()
		{
			UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
			UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnloaded;
		}

		private void OnApplicationQuit()
		{
			if (_saveOnApplicationQuit)
			{
				SaveGame();
			}
		}

		private List<ISaveableEntity> FindAllSaveDataObjects()
		{
			// Use LINQ to find all scripts that are using ISaveableEntity.
			IEnumerable<ISaveableEntity> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveableEntity>();
			return new List<ISaveableEntity>(dataPersistenceObjects);
		}

	}

	public class Placeholder
	{
		// Import the following methods when you want to Open/Load game data.
		public void OnNewGameClicked()
		{
			GameManager.instance.persistentDataManager.CreateNewSaveFile();
		}

		public void OnLoadGameClicked()
		{
			GameManager.instance.persistentDataManager.LoadGame();
		}

		public void OnSaveGameClicked()
		{
			GameManager.instance.persistentDataManager.SaveGame();
		}
	}

	public class MainMenu
	{
		/// <summary>
		/// This class is to be used any parent object which hold the buttons in the main menu
		/// </summary>

		private Button _newGameBtn;
		private Button _continueBtn;

		private void Start()
		{
			if (!GameManager.instance.persistentDataManager.HasGameData())
			{
				_continueBtn.interactable = false;
			}
		}

		public void OnNewGameBtnClicked()
		{
			DisableMenuBtns();

			// create a new game - which will initialize out game data
			GameManager.instance.persistentDataManager.CreateNewSaveFile();

			// Load gameplay scene - which will in turn save our game
			// because of OnSceneLoaded() in the PersistentDataManager.
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("SampleScene");
		}

		public void OnContinueBtnClicked()
		{
			DisableMenuBtns();

			// Load the next scene - which will in turn load our game
			// because of OnSceneLoaded() in the PersistentDataManager
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("SampleScene");
		}

		private void DisableMenuBtns()
		{
			_newGameBtn.interactable = false;
			_continueBtn.interactable = false;
		}
	}
}
