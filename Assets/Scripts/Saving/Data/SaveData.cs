using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Inventory;
using Arcy.Quests;
using UnityEngine;

namespace Arcy.Saving
{
	[System.Serializable]
	public class SaveData
	{
		/// <summary>
		/// This class contains all the variables that the saving system will use.
		/// IDEA - use a separate script for every namespace (player stats, quest progress, dialogue, inventory etc).
		/// </summary>

		public string mostRecentCheckpoint;
		public Quest[] questProgress;
		public InventorySlot[] inventory;
		public SerializableDictionary<string, bool> coinsCollected;

		// The values in his constructor will be our default values that the game starts with when there's no data to load.
		public SaveData()
		{
			mostRecentCheckpoint = "";
			coinsCollected = new SerializableDictionary<string, bool>();
		}
	}
}
