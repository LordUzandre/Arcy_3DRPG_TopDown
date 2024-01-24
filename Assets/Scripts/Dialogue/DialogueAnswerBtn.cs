using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Dialogue
{
	public class DialogueAnswerBtn : MonoBehaviour
	{
		[SerializeField] public Button btn;
		[SerializeField] public TMPro.TMP_Text txt;
		[SerializeField] public string[] btnText = new string[3];

#if UNITY_EDITOR
		private void OnValidate()
		{
			btn ??= TryGetComponent<Button>(out Button component) ? component : null;
			txt ??= btn.GetComponentInChildren<TMP_Text>();

			if (txt != null) txt.text = btnText[0];
		}
#endif
	}
}
