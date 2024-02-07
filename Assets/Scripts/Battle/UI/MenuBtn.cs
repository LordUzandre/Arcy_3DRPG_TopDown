using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.UI
{
	public class MenuBtn : MonoBehaviour
	{
		[Header("Non-selected images")]
		[SerializeField] private Sprite _disabledImage;
		[SerializeField] private Sprite _highlightedImage;
		[Header("selected system")]
		[SerializeField] private Sprite _highlightAndSelected;
		[SerializeField] private Sprite _selectedImage;

		private Image _imageElement;

		private void OnEnable()
		{
			_imageElement = TryGetComponent<Image>(out Image myImage) ? myImage : null;
		}

		public void OnSelected()
		{
			_imageElement.sprite = _selectedImage;
		}

		public void OnHighlighted()
		{

		}

		public void OnDeselected()
		{

		}
	}
}
