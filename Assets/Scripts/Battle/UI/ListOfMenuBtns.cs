using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.UI
{
	public class ListOfMenuBtns : MonoBehaviour
	{
		[SerializeField] private GameObject _parentObject;
		[SerializeField] public List<MenuBtn> listOfBtns = new List<MenuBtn>();

#if UNITY_EDITOR
		private void OnValidate()
		{
			CheckComponents();
		}
#endif

		private void CheckComponents()
		{
			if (listOfBtns.Count == 0)
			{
				if (_parentObject == null)
				{
					foreach (Transform child in transform)
					{
						if (child.TryGetComponent<MenuBtn>(out MenuBtn hit))
						{
							listOfBtns.Add(hit);
						}
					}
				}
				else
				{
					foreach (Transform child in _parentObject.transform)
					{
						if (child.TryGetComponent<MenuBtn>(out MenuBtn hit))
						{
							listOfBtns.Add(hit);
						}
					}
				}
			}
		}

	}
}
