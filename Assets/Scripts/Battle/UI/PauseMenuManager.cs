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

		private int _currentIndex;

		private MenuBtn _currentlySelectedBtn;
		private MenuBtn _previouslySelectedBtn;

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
					OpenNewMenu(_currentIndex);
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
					// TODO: Return to Header
					return;
				case MenuStates.CharacterMenu:
					// TODO: Return to Header
					return;
				case MenuStates.QuestLogMenu:
					// TODO: Return to Header
					return;
				default:
					return;
			}
		}

		private void OnEscapeBtnClicked()
		{
			// TODO: Return to game
		}

		private void OnRightInput()
		{
			// TODO: When the player clicks right-btn
		}

		private void OnLeftInput()
		{
			// When the player clicks left-btn
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
					menus.SetActive(false);
				}
				else
				{
					menus.SetActive(true);
				}
			}
		}

	}
}