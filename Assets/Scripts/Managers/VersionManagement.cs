using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Arcy.Management
{
	public class VersionManagement : MonoBehaviour
	{
#if UNITY_EDITOR
		private void OnValidate()
		{
			TextMeshProUGUI textMesh = GetComponentInChildren<TextMeshProUGUI>();

			textMesh.text = $"{PlayerSettings.productName} v{PlayerSettings.bundleVersion}-pre-alpha+{DateTime.Now.ToString("yyyMMdd")} © {PlayerSettings.companyName} 2024";
		}
#endif
	}
}
