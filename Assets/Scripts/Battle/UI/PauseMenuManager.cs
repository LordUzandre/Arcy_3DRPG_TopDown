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
		[SerializeField] private List<GameObject> allMenus = new List<GameObject>();

		[Header("Header Menu")]
		[SerializeField] private List<MenuBtn> _headerBtnList = new List<MenuBtn>();
		[Header("Settings Menu")]
		[SerializeField] private List<MenuBtn> _settingsBtnList = new List<MenuBtn>();
		[Header("Character Menu")]
		[SerializeField] private List<MenuBtn> _characterMenuLeftBtnList = new List<MenuBtn>();
		[SerializeField] private List<MenuBtn> _characterMenuRightBtnList = new List<MenuBtn>();
		[Header("Quest Menu")]
		[SerializeField] private List<MenuBtn> _questLogBtnList = new List<MenuBtn>();
		// private List<MenuBtn> _currentMenuList = new List<MenuBtn>();

		private int _currentHeaderIndex;
		private int _currentMenuIndex;

		private MenuBtn _currentlySelectedBtn;
		private MenuBtn _previouslySelectedBtn;

		private void OnEnable()
		{
			GameStateManager.OnGameStateChanged += OnGameStateChanged;
		}

		private void myMethod()
		{
			StartCoroutine(ShortDelay());

			IEnumerator ShortDelay()
			{
				yield return null;

				_currentHeaderIndex = 0;
				_currentlySelectedBtn = _headerBtnList[_currentHeaderIndex];
				_currentlySelectedBtn.OnSelected();

				Debug.Log(_currentHeaderIndex);
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
			InputManager.instance.WASDInput += InputVector;
		}

		private void UnSubscribeFromInputManager()
		{
			InputManager.instance.InteractionInputPressed -= OnInteractBtnClicked;
		}

		private void OnInteractBtnClicked()
		{
			// TODO: When we click the "enter"-button
			switch (_currentMenuState)
			{
				case MenuStates.Header:
					_currentlySelectedBtn.OnHighlighted();
					OpenNewMenu(_currentHeaderIndex);
					return;
				default:
					return;
			}
		}

		// When player clicks q-button
		private void OnBackBtnClicked()
		{
			switch (_currentMenuState)
			{
				case MenuStates.Header:
					// TODO: Leave the menu and return to game
					return;
				case MenuStates.SettingsMenu:
					SetPauseMenuState(MenuStates.Header);
					return;
				case MenuStates.CharacterMenu:
					SetPauseMenuState(MenuStates.Header);
					return;
				case MenuStates.QuestLogMenu:
					SetPauseMenuState(MenuStates.Header);
					return;
				default:
					return;
			}
		}

		private void SetPauseMenuState(MenuStates newMenuState)
		{
			if (newMenuState == _currentMenuState)
				return;

			_currentMenuState = newMenuState;

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

		private void OnEscapeBtnClicked()
		{
			// TODO: Return to game
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
			if (_currentlySelectedBtn?.OnRightSelect != null)
			{
				_currentlySelectedBtn.OnDeselected();
				_currentlySelectedBtn = _currentlySelectedBtn.OnRightSelect;
				_currentlySelectedBtn.OnSelected();
				_currentHeaderIndex = _headerBtnList.IndexOf(_currentlySelectedBtn);
			}
		}

		private void OnLeftInput()
		{
			if (_currentlySelectedBtn?.OnLeftSelect != null)
			{
				_currentlySelectedBtn.OnDeselected();
				_currentlySelectedBtn = _currentlySelectedBtn.OnLeftSelect;
				_currentlySelectedBtn.OnSelected();
				_currentHeaderIndex = _headerBtnList.IndexOf(_currentlySelectedBtn);
			}
		}

		private void OnUpInput()
		{
			// When the player clicks up-btn
		}

		private void OnDownInput()
		{
			// When the player clicks down-btn
		}

		private void OpenNewMenu(int headerMenuIndex)
		{
			int localInt = 0;

			foreach (GameObject menus in allMenus)
			{
				localInt++;

				if (localInt != headerMenuIndex)
				{
					break;
				}
				else
				{
					menus.SetActive(true);
				}
			}
		}

		private List<MenuBtn> _currentMenuList(List<MenuBtn> myValues)
		{
			return myValues;
		}

	}
}