using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Saving
{
	[System.Serializable]
	public class GameData
	{
		/// <summary>
		/// This class contains all the variables that the saving system will save
		/// </summary>

		public int deathCount;
		public Vector3 playerPosition;

		// The values in his constructor will be our default values that the game starts with
		// When there's no data to load.
		public GameData()
		{
			this.deathCount = 0;
			playerPosition = Vector3.zero;
		}
	}
}
