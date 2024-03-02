using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.InputManagement;
using Arcy.Quests;
using UnityEditor;

namespace Arcy.UI
{
	public class PauseMenuManager : MonoBehaviour
	{
		private enum MenuStates { Header, SettingsMenu, CharacterMenu, ItemsInventory, QuestLogMenu, Map }
		private MenuStates _currentMenuState;

		public Action OnSelected;
		public Action OnDeselected;

		[Header("")]
		[SerializeField] private GameObject _uiParentObject;

		[Header("Menu-GameObjects")]
		[SerializeField] private GameObject _headerBarObject;
		[SerializeField] private GameObject _settingsMenuObject;
		[SerializeField] private GameObject _characterMenuObject;
		[SerializeField] private GameObject _inventoryMenuObject;
		[SerializeField] private GameObject _questLogMenuObject;
		[SerializeField] private GameObject _mapMenuObject;
		[Space]
		[SerializeField] private List<GameObject> _allMenuGameObjects = new List<GameObject>();

		// All btns available in each menu
		[Header("Header Menu")]
		[SerializeField] private List<MenuBtn> _headerBtnList = new List<MenuBtn>();
		[Header("Settings Menu")]
		[SerializeField] private List<MenuBtn> _settingsBtnList = new List<MenuBtn>();
		[Header("Items Menu")]
		[SerializeField] private List<MenuBtn> _itemsBtnsList = new List<MenuBtn>();
		[Header("Character Menu")]
		[SerializeField] private List<MenuBtn> _characterMenuLeftBtnList = new List<MenuBtn>();
		[SerializeField] private List<MenuBtn> _characterMenuRightBtnList = new List<MenuBtn>();
		[Header("Quest Menu")]
		[SerializeField] private List<MenuBtn> _questLogBtnList = new List<MenuBtn>();

		[Space]
		[SerializeField] private List<MenuBtn> _currentSubMenuList = new List<MenuBtn>();
		private List<List<MenuBtn>> _listWithAllMenus = new List<List<MenuBtn>>();

		// This int is based on which menu-btn is currently selected and decides which corresponding menu should be opened.
		// 0 = settings, 1 = character Menu, 2 = Quest Log etc.
		[Space]
		[SerializeField] private int _currentHeaderIndex;
		private MenuBtn _currentlySelectedBtn;
		private bool _pauseMenuIsActive;

#if UNITY_EDITOR
		private void OnValidate()
		{
			CheckComponents();
		}
#endif

		private void Start()
		{
			_pauseMenuIsActive = _uiParentObject.activeInHierarchy;

			GameStateManager.OnGameStateChanged += OnGameStateChanged;
			StartCoroutine(PopulateAlllistsCoroutine());

			IEnumerator PopulateAlllistsCoroutine()
			{
				yield return null;
				PopulateAllLists();
			}
		}

		private void OnDisable()
		{
			GameStateManager.OnGameStateChanged -= OnGameStateChanged;
		}

		private void OnGameStateChanged(GameState newGameState)
		{
			switch (newGameState)
			{
				case GameState.Pause:
					OpenPauseMenu();
					return;
				default:
					if (_pauseMenuIsActive)
					{
						CloseDownPauseMenu();
					}
					return;
			}
		}

		#region Components
		private void CheckComponents()
		{
			if (_uiParentObject == null)
			{
				_uiParentObject = GameObject.Find("PauseMenuCanvas");
			}

			if (_uiParentObject != null)
			{
				#region Transforms
				if (_headerBarObject == null)
				{
					Transform tempTransform = _uiParentObject.transform.Find("HeaderBar");
					_headerBarObject = tempTransform.gameObject;
				}

				if (_settingsMenuObject == null)
				{
					Transform tempTransform = _uiParentObject.transform.Find("SettingsWindow");
					_settingsMenuObject = tempTransform.gameObject;
				}

				if (_characterMenuObject == null)
				{
					Transform tempTransform = _uiParentObject.transform.Find("CharacterWindow");
					_characterMenuObject = tempTransform.gameObject;
				}

				if (_inventoryMenuObject == null)
				{
					Transform tempTransform = _uiParentObject.transform.Find("ItemsInventory");
					_inventoryMenuObject = tempTransform.gameObject;
				}

				if (_questLogMenuObject == null)
				{
					Transform tempTransform = _uiParentObject.transform.Find("QuestWindow");
					_questLogMenuObject = tempTransform.gameObject;
				}

				if (_mapMenuObject == null)
				{
					Transform tempTransform = _uiParentObject.transform.Find("MapWindow");
					_mapMenuObject = tempTransform.gameObject;
				}
				#endregion

				PopulateAllLists();

			}
		}

		private void PopulateAllLists()
		{
			PopulateList(_headerBarObject, _headerBtnList);
			PopulateList(_settingsMenuObject, _settingsBtnList);
			PopulateList(_inventoryMenuObject, _itemsBtnsList);
			PopulateList(_questLogMenuObject, _questLogBtnList);
			// Remember: add the map

			_allMenuGameObjects.Clear();
			_allMenuGameObjects.Add(_settingsMenuObject);
			_allMenuGameObjects.Add(_characterMenuObject);
			_allMenuGameObjects.Add(_inventoryMenuObject);
			_allMenuGameObjects.Add(_questLogMenuObject);
			_allMenuGameObjects.Add(_mapMenuObject);

			// All the menus
			_listWithAllMenus.Clear();
			_listWithAllMenus.Add(_headerBtnList);
			_listWithAllMenus.Add(_settingsBtnList);
			_listWithAllMenus.Add(_characterMenuLeftBtnList);
			_listWithAllMenus.Add(_characterMenuRightBtnList);
			_listWithAllMenus.Add(_questLogBtnList);
			// Remember: add the map
		}

		private void PopulateList(GameObject gameObjectWithList, List<MenuBtn> listToBePopulated)
		{
			if (gameObjectWithList.TryGetComponent<ListOfMenuBtns>(out ListOfMenuBtns ogListOfMenuBtns))
			{
				listToBePopulated.Clear();
				listToBePopulated.RemoveRange(0, listToBePopulated.Count);
				listToBePopulated.InsertRange(0, ogListOfMenuBtns.listOfBtns);
				listToBePopulated = ogListOfMenuBtns.listOfBtns;
			}
		}
		#endregion

		// Currently only opens on the default state of 'Settings'
		private void OpenPauseMenu(int myInt = 0)
		{
			SubscribeToInputManager();
			CheckComponents();

			_uiParentObject.SetActive(true);
			_pauseMenuIsActive = true;
			_currentHeaderIndex = myInt;
			_currentlySelectedBtn = _headerBtnList[_currentHeaderIndex];
			_currentlySelectedBtn.OnSelected();
			ShowSubMenu(_currentHeaderIndex);
		}

		private void CloseDownPauseMenu()
		{
			UnSubscribeFromInputManager();
			CheckComponents();
			_pauseMenuIsActive = false;
			_uiParentObject.SetActive(false);
		}

		#region Subscriptions

		// When we enter Pause-state
		private void SubscribeToInputManager()
		{
			// TODO: Set up a list of subscriptions to inputmanager
			InputManager.instance.InteractionInputPressed += OnInteractBtnClicked;
			InputManager.instance.CancelInputPressed += OnBackBtnClicked;
			InputManager.instance.WASDInput += InputVector;
			InputManager.instance.PauseInputPressed += OnEscapeBtnClicked;
		}

		private void UnSubscribeFromInputManager()
		{
			InputManager.instance.InteractionInputPressed -= OnInteractBtnClicked;
			InputManager.instance.CancelInputPressed -= OnBackBtnClicked;
			InputManager.instance.WASDInput -= InputVector;
			InputManager.instance.PauseInputPressed -= OnEscapeBtnClicked;
		}

		#endregion

		#region Input-methods

		private void OnInteractBtnClicked()
		{
			// TODO: When we click the "enter" or "e"-button
			switch (_currentMenuState)
			{
				case MenuStates.Header:
					_currentlySelectedBtn.OnHighlighted();
					return;
				default:
					_currentlySelectedBtn.OnHighlighted();
					return;
			}
		}

		// When player clicks q-button
		private void OnBackBtnClicked()
		{
			switch (_currentMenuState)
			{
				case MenuStates.Header:
					CloseDownPauseMenu();
					return;
				case MenuStates.SettingsMenu:
					SetPauseMenuState(MenuStates.Header);
					ChooseNewBtn(_headerBtnList[_currentHeaderIndex]);
					return;
				case MenuStates.CharacterMenu:
					SetPauseMenuState(MenuStates.Header);
					ChooseNewBtn(_headerBtnList[_currentHeaderIndex]);
					return;
				case MenuStates.QuestLogMenu:
					SetPauseMenuState(MenuStates.Header);
					ChooseNewBtn(_headerBtnList[_currentHeaderIndex]);
					return;
				default:
					return;
			}
		}

		private void OnEscapeBtnClicked()
		{
			if (GameStateManager.Instance.CurrentGameState == GameState.Pause)
			{
				GameStateManager.Instance.SetState(GameState.Freeroam);
				CloseDownPauseMenu();
			}
		}

		private void InputVector(Vector2 ogInputVector)
		{
			float xFloat = ogInputVector.x;
			float yFloat = ogInputVector.y;

			if (xFloat != 0)
			{
				if (xFloat < -0.5f)
				{
					OnLeftInput();
				}
				else if (xFloat > 0.5f)
				{
					OnRightInput();
				}
			}
			else if (yFloat != 0)
			{
				if (yFloat < -0.5f)
				{
					OnDownInput();
				}
				else if (yFloat > 0.5f)
				{
					OnUpInput();
				}
			}
		}

		private void OnRightInput()
		{
			switch (_currentMenuState)
			{
				case MenuStates.Header:
					if (_currentlySelectedBtn?.OnRightSelect != null)
					{
						ChooseNewBtn(_currentlySelectedBtn.OnRightSelect);
						_currentHeaderIndex = _headerBtnList.IndexOf(_currentlySelectedBtn);
						ShowSubMenu(_currentHeaderIndex);
					}
					break;
			}
		}

		private void OnLeftInput()
		{
			switch (_currentMenuState)
			{
				case MenuStates.Header:
					if (_currentlySelectedBtn?.OnLeftSelect != null)
					{
						ChooseNewBtn(_currentlySelectedBtn.OnLeftSelect);
						_currentHeaderIndex = _headerBtnList.IndexOf(_currentlySelectedBtn);
						ShowSubMenu(_currentHeaderIndex);
					}
					return;
			}
		}

		private void OnUpInput()
		{
			if (_currentlySelectedBtn == _currentSubMenuList[0])
			{
				SetPauseMenuState(MenuStates.Header);
				return;
			}

			// When the player clicks up-btn
			switch (_currentMenuState)
			{
				case MenuStates.SettingsMenu:
					if (_currentlySelectedBtn?.OnUpSelect != null)
						ChooseNewBtn(_currentlySelectedBtn.OnUpSelect);
					return;
				default:
					return;
			}
		}

		private void OnDownInput()
		{
			// When the player clicks down-btn
			switch (_currentMenuState)
			{
				case MenuStates.Header:
					if (_currentlySelectedBtn?.OnDownSelect != null)
					{
						SetPauseMenuState(MenuStateBasedOnInt(_currentHeaderIndex + 1));
					}
					return;
				case MenuStates.SettingsMenu:
					if (_currentlySelectedBtn?.OnDownSelect != null)
					{
						if (_currentlySelectedBtn.OnDownSelect.isInteractible)
						{
							ChooseNewBtn(_currentlySelectedBtn.OnDownSelect);
						}
					}
					return;
				case MenuStates.QuestLogMenu:
					if (false)
					{

					}
					else if (true)
					{

					}
					return;
				default:
					return;
			}
		}

		#endregion

		private void ChooseNewBtn(MenuBtn newBtn)
		{
			_currentlySelectedBtn.OnDeselected();
			_currentlySelectedBtn = newBtn;
			_currentlySelectedBtn.OnSelected();
		}

		// Open a new Menu Window and assign the appropriate list. Should not select a new btn
		private void ShowSubMenu(int currentHeaderIndex)
		{
			for (int i = 0; i < _allMenuGameObjects.Count; i++)
			{
				if (i == currentHeaderIndex)
				{
					// Show the current sub-menu
					_allMenuGameObjects[i].SetActive(true);
					_currentSubMenuList = _listWithAllMenus[i + 1];
				}
				else
				{
					// Inactivate all other menus
					_allMenuGameObjects[i].SetActive(false);
				}
			}
		}

		private MenuStates MenuStateBasedOnInt(int headerIndex)
		{
			switch (headerIndex)
			{
				case 0:
					return MenuStates.Header;
				case 1:
					return MenuStates.SettingsMenu;
				case 2:
					return MenuStates.CharacterMenu;
				case 3:
					return MenuStates.ItemsInventory;
				case 4:
					return MenuStates.QuestLogMenu;
				case 5:
					return MenuStates.Map;
				default:
					Debug.LogWarning("MenuBasedOnInt - Something went wrong!");
					return MenuStates.Header;
			}
		}

		private void SetPauseMenuState(MenuStates newMenuState)
		{
			if (newMenuState == _currentMenuState)
			{
				Debug.LogWarning("SetState Failed");
				return;
			}

			_currentMenuState = newMenuState;

			switch (_currentMenuState)
			{
				case MenuStates.Header:
					ChooseNewBtn(_headerBtnList[_currentHeaderIndex]);
					return;
				case MenuStates.SettingsMenu:
					ChooseNewBtn(_settingsBtnList[0]);
					return;
				case MenuStates.CharacterMenu:
					ChooseNewBtn(_characterMenuLeftBtnList[0]);
					return;
				case MenuStates.QuestLogMenu:
					// if (true)
					// {
					// ChooseNewBtn();
					// }
					// else if (true)
					// {

					// }
					return;
				default:
					return;
			}
		}

	}
}