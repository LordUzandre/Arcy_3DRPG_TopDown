using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Dialogue;
using Arcy.InputManagement;
using Arcy.Inventory;
using Arcy.Quests;
using Arcy.Player;
using UnityEngine;
using Arcy.Scenes;

namespace Arcy.Management
{
	public class GameEventManager : MonoBehaviour
	{
		// public static GameEventManager instance { get; private set; }

		public InputEvents inputEvents;

		public CheckpointEvents checkpointEvents;
		public DialogueEvents dialogueEvents;
		public InteractionEvents interactionEvents;
		public InventoryEvents inventoryEvents;
		public PlayerEvents playerEvents;
		public QuestEvents questEvents;

		public void Initialize()
		{
			//initialize all events
			inputEvents = new InputEvents();

			checkpointEvents = new CheckpointEvents();
			dialogueEvents = new DialogueEvents();
			interactionEvents = new InteractionEvents();
			inventoryEvents = new InventoryEvents();
			playerEvents = new PlayerEvents();
			questEvents = new QuestEvents();
		}

	}
}
