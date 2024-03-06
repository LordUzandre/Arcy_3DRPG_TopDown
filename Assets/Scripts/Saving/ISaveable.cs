using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Saving
{
	public interface ISaveable
	{
		void LoadData(GameData data);
		void SaveData(GameData data);
	}
}
