using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Dialogue;
using Arcy.InputManagement;
using Arcy.Inventory;
using Arcy.Quests;
using Arcy.Player;
using UnityEngine;
using Arcy.Saving;

namespace Arcy.Management
{
	public class GameEventManager : MonoBehaviour
	{
		// public static GameEventManager instance { get; private set; }

		public InputEvents inputEvents;

		public PlayerEvents playerEvents;
		public QuestEvents questEvents;
		public InventoryEvents inventoryEvents;
		public DialogueEvents dialogueEvents;
		public CheckpointEvents checkpointEvents;

		public void Initialize()
		{
			//initialize all events
			inputEvents = new InputEvents();
			playerEvents = new PlayerEvents();
			questEvents = new QuestEvents();
			inventoryEvents = new InventoryEvents();
			dialogueEvents = new DialogueEvents();
			checkpointEvents = new CheckpointEvents();
		}
	}
}
