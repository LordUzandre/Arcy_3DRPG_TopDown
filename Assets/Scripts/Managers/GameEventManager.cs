using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Dialogue;
using Arcy.InputManagement;
using Arcy.Inventory;
using Arcy.Quests;
using UnityEngine;

namespace Arcy.Management
{
	public class GameEventManager : MonoBehaviour
	{
		public static GameEventManager instance { get; private set; }

		// Subscribe to events:
		public InputEvents inputEvents;
		public QuestEvents questEvents;
		public InventoryEvents inventoryEvents;
		public GameStateManager gameStateManager;
		public DialogueEvents dialogueEvents;
		// DialogueEvents

		private void Awake()
		{
			if (instance != null)
			{
				Debug.LogError("Found more than one Game Events Manager in the scene.");
			}
			instance = this;

			//initialize all events
			inputEvents = new InputEvents();
			questEvents = new QuestEvents();
			inventoryEvents = new InventoryEvents();
			gameStateManager = new GameStateManager();
			dialogueEvents = new DialogueEvents();
		}
	}
}
