using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Saving
{
	public interface ISaveableEntity
	{
		/// <summary>
		/// This interface is used by every monobehaviour that has data that is going to be saved.
		/// Remember that a corresponding variable need to exist in SaveData
		/// </summary>

		void LoadData(SaveData data);
		void SaveData(SaveData data);
	}
}
