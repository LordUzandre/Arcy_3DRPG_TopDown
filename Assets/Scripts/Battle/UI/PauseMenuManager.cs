using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.InputManagement;

namespace Arcy.UI
{
	public class PauseMenuManager : MonoBehaviour
	{
		private enum MenuStates { Header, SettingsMenu, CharacterMenu, QuestLogMenu }
		private MenuStates _currentMenuState;

		public Action OnSelected;
		public Action OnDeselected;

		[Header("Menu-GameObjects")]
		[SerializeField] private GameObject _headerObject;
		[SerializeField] private GameObject _settingsMenu;
		[SerializeField] private GameObject _characterMenu;
		[SerializeField] private GameObject _questLogMenu;
		[SerializeField] private List<GameObject> _allMenuGameObjects = new List<GameObject>();

		// All btns available in each menu
		[Header("Header Menu")]
		[SerializeField] private List<MenuBtn> _headerBtnList = new List<MenuBtn>();
		[Header("Settings Menu")]
		[SerializeField] private List<MenuBtn> _settingsBtnList = new List<MenuBtn>();
		[Header("Character Menu")]
		[SerializeField] private List<MenuBtn> _characterMenuLeftBtnList = new List<MenuBtn>();
		[SerializeField] private List<MenuBtn> _characterMenuRightBtnList = new List<MenuBtn>();
		[Header("Quest Menu")]
		[SerializeField] private List<MenuBtn> _questLogBtnList = new List<MenuBtn>();

		[Space]
		private List<List<MenuBtn>> _listWithAllMenus = new List<List<MenuBtn>>();
		[SerializeField] private List<MenuBtn> _currentSubMenuList = new List<MenuBtn>();

		// This int is based on which menu-btn is currently selected and decides which corresponding menu should be opened.
		// 0 = settings, 1 = character Menu, 2 = Quest Log etc.
		private int _currentHeaderIndex;
		private MenuBtn _currentlySelectedBtn;

		private void Start()
		{
			_allMenuGameObjects.Clear();
			//_allMenuGameObjects.Add(_headerObject);
			_allMenuGameObjects.Add(_settingsMenu);
			_allMenuGameObjects.Add(_characterMenu);
			_allMenuGameObjects.Add(_questLogMenu);

			// All the menus
			_listWithAllMenus.Clear();
			_listWithAllMenus.Add(_headerBtnList);
			_listWithAllMenus.Add(_settingsBtnList);
			_listWithAllMenus.Add(_characterMenuLeftBtnList);
			_listWithAllMenus.Add(_characterMenuRightBtnList);
			_listWithAllMenus.Add(_questLogBtnList);
		}

		private void OnEnable()
		{
			GameStateManager.OnGameStateChanged += OnGameStateChanged;
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
					SubscribeToInputManager();
					return;
				default:
					UnSubscribeFromInputManager();
					return;
			}
		}

		// Can currently only open on the default state of 'Settings'
		private void OpenPauseMenu()
		{
			_currentHeaderIndex = 0;
			_currentlySelectedBtn = _headerBtnList[_currentHeaderIndex];
			_currentlySelectedBtn.OnSelected();
			ShowSubMenu(_currentHeaderIndex);
		}

		#region Subscriptions

		// When we enter Pause-state
		private void SubscribeToInputManager()
		{
			// TODO: Set up a list of subscriptions to inputmanager
			InputManager.instance.InteractionInputPressed += OnInteractBtnClicked;
			InputManager.instance.CancelInputPressed += OnBackBtnClicked;
			InputManager.instance.WASDInput += InputVector;
		}

		private void UnSubscribeFromInputManager()
		{
			InputManager.instance.InteractionInputPressed -= OnInteractBtnClicked;
			InputManager.instance.CancelInputPressed -= OnBackBtnClicked;
			InputManager.instance.WASDInput -= InputVector;
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
			CloseDownPauseMenu();
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
				Debug.Log("Return to header");
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
					// Debug.Log($"i = '{i}', _currentHeaderIndex = '{_currentHeaderIndex}', currently selected btn = '{_currentlySelectedBtn.name}', \n currently showing menu = '{_allMenuGameObjects[i].name}'");
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
					Debug.Log("Header-state by int");
					return MenuStates.Header;
				case 1:
					Debug.Log("Settings-state by int");
					return MenuStates.SettingsMenu;
				case 2:
					Debug.Log("Char-state by int");
					return MenuStates.CharacterMenu;
				case 3:
					Debug.Log("Quest-state by int");
					return MenuStates.QuestLogMenu;
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
					Debug.Log("Settings Menu State");
					return;
				case MenuStates.CharacterMenu:
					ChooseNewBtn(_characterMenuLeftBtnList[0]);
					return;
				case MenuStates.QuestLogMenu:
					return;
				default:
					return;
			}
		}

		private void CloseDownPauseMenu()
		{
			// TODO: Reset all faculties and return to the game.
			// Remember to reset Input Action Map.
		}

	}
}