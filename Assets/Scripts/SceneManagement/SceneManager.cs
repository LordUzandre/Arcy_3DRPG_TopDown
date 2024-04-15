using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.SceneManagement;
using UnityEngine;

namespace Arcy.SceneManager
{
	public class SceneManager : MonoBehaviour
	{
		[SerializeField] CheckPoint[] _allCheckpointsInScene;
		[Space]
		[SerializeField] CheckPoint _mostRecentCheckpoint;
		[SerializeField] string _mostRecentCheckpointGUID;
	}
}
