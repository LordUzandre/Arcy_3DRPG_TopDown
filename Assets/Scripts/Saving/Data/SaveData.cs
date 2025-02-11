using System.Collections;
using System.Collections.Generic;
using Arcy.Utils;

namespace Arcy.Saving
{
	[System.Serializable]
	public class SaveData
	{
		/// <summary>
		/// This class contains all the variables that the saving system will use.
		/// IDEA - use a separate script for every namespace (player stats, quest progress, dialogue, inventory etc).
		/// </summary>

		public int mostRecentCheckpoint;
		// Inventory:
		public int inventorySize;
		public SerializableDictionary<int, bool> pickupsCollected; // pickupsGuid + ifCollected
		public SerializableDictionary<int, int> inventory; // itemGuid + amount
		public string[] inventoryString;

		// Quests
		public SerializableDictionary<int, string> questLog;

		// The values in his constructor will be our default values that the game starts with when there's no data to load.
		public SaveData()
		{
			mostRecentCheckpoint = 0;

			// Inventory:
			inventorySize = 0;
			pickupsCollected = new SerializableDictionary<int, bool>();
			inventory = new SerializableDictionary<int, int>();
			inventoryString = new string[0];

			// Quests:
			questLog = new SerializableDictionary<int, string>();
		}
	}
}
