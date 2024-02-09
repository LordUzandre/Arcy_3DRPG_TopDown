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

		[Header("Explicit")]
		[SerializeField] public MenuBtn OnUpSelect;
		[SerializeField] public MenuBtn OnLeftSelect;
		[SerializeField] public MenuBtn OnRightSelect;
		[SerializeField] public MenuBtn OnDownSelect;

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
			_imageElement.sprite = _highlightedImage;
		}

		public void OnDeselected()
		{
			_imageElement.sprite = _disabledImage;
		}
	}
}
