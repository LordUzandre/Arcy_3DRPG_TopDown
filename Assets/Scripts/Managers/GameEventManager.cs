using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Dialogue;
using Arcy.InputManagement;
using Arcy.Inventory;
using Arcy.Quests;
using Arcy.Player;
using UnityEngine;

namespace Arcy.Management
{
	public class GameEventManager : MonoBehaviour
	{
		public static GameEventManager instance { get; private set; }

		// input 
		public InputEvents inputEvents;
		// gamestate-manager
		public GameStateManager gameStateManager;

		public PlayerEvents playerEvents;
		public QuestEvents questEvents;
		public InventoryEvents inventoryEvents;
		public DialogueEvents dialogueEvents;

		public void Awake()
		{
			if (instance != null)
			{
				Debug.LogError("Found more than one Game Events Manager in the scene.");
			}

			instance = this;

			//initialize all events
			inputEvents = new InputEvents();
			gameStateManager = new GameStateManager();
			playerEvents = new PlayerEvents();
			questEvents = new QuestEvents();
			inventoryEvents = new InventoryEvents();
			dialogueEvents = new DialogueEvents();
		}
	}
}
