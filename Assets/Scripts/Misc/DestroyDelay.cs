using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{
	public float lifetime;

	private void Start()
	{
		Destroy(gameObject, lifetime);
	}
}
