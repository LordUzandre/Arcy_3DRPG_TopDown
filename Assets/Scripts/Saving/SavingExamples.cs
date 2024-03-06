using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Saving
{
	public class SavingExamples : MonoBehaviour, ISaveable
	{
		// Below is only an example of the methods a class need to implement in order to use the saving system
		[SerializeField] private int _deathCount = 3;

		public void LoadData(GameData data)
		{
			_deathCount = data.deathCount;
		}

		public void SaveData(GameData data)
		{
			data.deathCount = _deathCount;
		}
	}
}
