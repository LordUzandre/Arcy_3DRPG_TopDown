using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Saving
{
	public class SavingExamples : MonoBehaviour, ISaveableEntity
	{
		[SerializeField] string _mostRecentCheckPoint;

		public void LoadData(SaveData saveData)
		{
			_mostRecentCheckPoint = saveData.mostRecentCheckpoint;

		}

		public void SaveData(SaveData saveData)
		{
			saveData.mostRecentCheckpoint = _mostRecentCheckPoint;
		}
	}
}
