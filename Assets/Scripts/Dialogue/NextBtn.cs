using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Dialogue
{
	public class NextBtn : MonoBehaviour
	{
		[SerializeField] private GameObject _nextBtn;

		private void OnEnable()
		{
			TypewriterEffect.CompleteTextRevealed += ShowNextBtn;
		}

		private void OnDisable()
		{
			TypewriterEffect.CompleteTextRevealed -= ShowNextBtn;
		}

		private void ShowNextBtn()
		{
			_nextBtn?.SetActive(true);
		}

		public void HideNextBtn()
		{
			_nextBtn?.SetActive(false);
		}
	}
}
