using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Arcy.Battle
{
	public class ActionBtnEventTrigger : EventTrigger
	{
		public override void OnPointerClick(PointerEventData data)
		{
			Debug.Log("OnPointerClick called.");
		}

		public override void OnPointerEnter(PointerEventData data)
		{
			Debug.Log("OnPointerEnter called.");
		}

		public override void OnPointerExit(PointerEventData data)
		{
			Debug.Log("OnPointerExit called.");
		}

		public override void OnSelect(BaseEventData data)
		{
			Debug.Log("OnSelect called.");
		}
	}
}
