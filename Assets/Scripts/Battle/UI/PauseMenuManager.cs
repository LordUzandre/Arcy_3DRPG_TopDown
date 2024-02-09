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
		[SerializeField] private List<GameObject> _allMenuObjects = new List<GameObject>();

		// ALl the btns available in each menu
		[Header("Header Menu")]
		[SerializeField] private List<MenuBtn> _headerBtnList = new List<MenuBtn>();
		[Header("Settings Menu")]
		[SerializeField] private List<MenuBtn> _settingsBtnList = new List<MenuBtn>();
		[Header("Character Menu")]
		[SerializeField] private List<MenuBtn> _characterMenuLeftBtnList = new List<MenuBtn>();
		[SerializeField] private List<MenuBtn> _characterMenuRightBtnList = new List<MenuBtn>();
		[Header("Quest Menu")]
		[SerializeField] private List<MenuBtn> _questLogBtnList = new List<MenuBtn>();

		private List<List<MenuBtn>> _allMenusList = new List<List<MenuBtn>>();
		private List<MenuBtn> _currentMenuList = new List<MenuBtn>();

		// This int is based on which menu-btn is currently selected and decides which corresponding menu should be opened.
		// 0 = settings, 1 = character Menu, 2 = Quest Log etc.
		private int _currentHeaderIndex;
		private int _currentMenuIndex;

		private MenuBtn _currentlySelectedBtn;
		private MenuBtn _previouslySelectedBtn;

		private void Start()
		{
			if (_allMenuObjects.Count == 0)
			{
				_allMenuObjects.Add(_headerObject);
				_allMenuObjects.Add(_settingsMenu);
				_allMenuObjects.Add(_characterMenu);
				_allMenuObjects.Add(_questLogMenu);
			}

			// All the menus
			if (_allMenusList.Count == 0)
			{
				_allMenusList.Add(_headerBtnList);
				_allMenusList.Add(_settingsBtnList);
				_allMenusList.Add(_characterMenuLeftBtnList);
				_allMenusList.Add(_characterMenuRightBtnList);
				_allMenusList.Add(_questLogBtnList);
			}
		}

		private void OnEnable()
		{
			GameStateManager.OnGameStateChanged += OnGameStateChanged;
		}

		private void myMethod() // For Debug purposes. Remove when implementation is happening
		{
			StartCoroutine(ShortDelay());

			IEnumerator ShortDelay()
			{
				yield return null;

				_currentHeaderIndex = 0;
				OpenNewMenu(_currentHeaderIndex);
				_currentlySelectedBtn = _headerBtnList[_currentHeaderIndex];
				_currentlySelectedBtn.OnSelected();
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
					myMethod();
					SubscribeToInputManager();
					return;
				default:
					UnSubscribeFromInputManager();
					return;
			}
		}

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
						OpenNewMenu(_currentHeaderIndex);
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
						OpenNewMenu(_currentHeaderIndex);
					}
					return;
			}
		}

		private void OnUpInput()
		{
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
			Debug.Log("On Down Input");

			// When the player clicks down-btn
			switch (_currentMenuState)
			{
				case MenuStates.Header:
					if (_currentlySelectedBtn?.OnDownSelect != null)
					{
						ChooseNewBtn(_currentMenuList[0]);
					}
					return;
				case MenuStates.SettingsMenu:
					if (_currentlySelectedBtn?.OnDownSelect != null)
						ChooseNewBtn(_currentlySelectedBtn.OnDownSelect);
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

		// Open a new Menu Window and assign the appropriate list Should not select a new btn
		private void OpenNewMenu(int headerMenuIndex)
		{
			SetPauseMenuState(SetStateBasedOnInt(headerMenuIndex));

			foreach (GameObject menuObject in _allMenuObjects)
			{
				int localInt = 0;

				if (headerMenuIndex == localInt)
				{
					menuObject.SetActive(true);
					_currentMenuList = _allMenusList[localInt];
					localInt++;
					break;
				}
				else
				{
					// Inactivate all other menus (except header)
					if (localInt != 0)
					{
						menuObject.SetActive(false);
					}
					localInt++;
					break;
				}
			}
		}

		private MenuStates SetStateBasedOnInt(int headerIndex)
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
					return MenuStates.QuestLogMenu;
				default:
					Debug.Log("Something went wrong");
					return MenuStates.Header;
			}
		}

		private void SetPauseMenuState(MenuStates newMenuState)
		{
			if (newMenuState == _currentMenuState)
				return;

			_currentMenuState = newMenuState;
			Debug.Log($"new Menu State = {_currentMenuState}");

			switch (_currentMenuState)
			{
				case MenuStates.Header:
					int listIndex = 0;

					foreach (MenuBtn btn in _headerBtnList)
					{
						listIndex++;

						if (listIndex == _headerBtnList.IndexOf(btn))
						{
							_headerBtnList[_currentHeaderIndex].OnSelected();
						}
						else
						{
							btn.OnDeselected();
						}
					}
					return;
				case MenuStates.SettingsMenu:
					return;
				case MenuStates.CharacterMenu:
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