using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BoneRenderScript : MonoBehaviour
{
	[SerializeField] private BoneRenderer _boneRenderer;
	[SerializeField] private Transform _rigParent;

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (_boneRenderer == null)
			_boneRenderer = GetComponent<BoneRenderer>();

		if (_boneRenderer != null && _boneRenderer.transforms.Length == 0 && _rigParent != null)
		{
			_boneRenderer.transforms = GetAllChildObjects(_rigParent);
		}
	}

	// Recursive method to get all child objects
	Transform[] GetAllChildObjects(Transform rigParent)
	{
		// List to store all child objects
		List<Transform> allChildren = new List<Transform>();

		// Add immediate children
		for (int i = 0; i < rigParent.childCount; i++)
		{
			Transform child = rigParent.GetChild(i);
			allChildren.Add(child);

			// Recursively add child objects of the current child
			allChildren.AddRange(GetAllChildObjects(child));
		}

		return allChildren.ToArray();
	}
#endif
}
