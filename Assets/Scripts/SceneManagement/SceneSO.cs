using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arcy.Scenes
{
	[CreateAssetMenu(fileName = "SceneSO", menuName = "Arcy/Scene Management/new Scene")]
	public class SceneSO : ScriptableObject
	{
		public int sceneNumberGuid = 0;
		public TorchCheckpoint[] checkPoints;

		private void OnValidate()
		{

		}
	}
}