using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Arcy.Utils
{
	public static class GuidGenerator
	{
		public static int guid()
		{
			int guidInt = Mathf.Abs(System.Guid.NewGuid().GetHashCode());
			return guidInt;
		}

		public static int guid(GameObject gObject)
		{
			int guidInt = Mathf.Abs(System.Guid.NewGuid().GetHashCode());
			return guidInt;
		}

		public static int guid(ScriptableObject scriptableObject)
		{
			int guidInt = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(scriptableObject)).GetHashCode();
			return guidInt;
		}
	}
}
