using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializeableDictionary : MonoBehaviour
{
	// [SerializeField] public string thisObjectName;
	[SerializeField] NewDictionary newDict;
	[SerializeField] Dictionary<string, GameObject> objectNames;

	private void Start()
	{
		objectNames = newDict.ToDictionary();
	}
}

[System.Serializable]
public class NewDictionary
{
	[SerializeField] NewDictionaryItem[] thisDictItems;

	public Dictionary<string, GameObject> ToDictionary()
	{
		Dictionary<string, GameObject> newDictionary = new Dictionary<string, GameObject>();

		foreach (NewDictionaryItem item in thisDictItems)
		{
			newDictionary.Add(item.name, item.obj);
		}

		return newDictionary;
	}
}

[System.Serializable]
public class NewDictionaryItem
{
	[SerializeField]
	public string name;
	[SerializeField]
	public GameObject obj;
}