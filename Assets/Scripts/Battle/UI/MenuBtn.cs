using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

		[HideInInspector] public bool isInteractible = true;
		[HideInInspector] public bool isHighlighted = false;

		private Image _imageElement;
		private TMP_Text _textMeshPro;

		private void OnEnable()
		{
			_imageElement = TryGetComponent<Image>(out Image myImage) ? myImage : null;
			_textMeshPro = GetComponentInChildren<TMP_Text>();
		}

		public void OnSelected()
		{
			if (!isHighlighted)
			{
				_imageElement.sprite = _selectedImage;
			}
			else
			{
				_imageElement.sprite = _highlightAndSelected;
			}
		}

		public void OnHighlighted()
		{
			_imageElement.sprite = _highlightedImage;
		}

		public void OnDeselected()
		{
			if (!isHighlighted)
			{
				_imageElement.sprite = _disabledImage;
			}
			else
			{
				_imageElement.sprite = _highlightedImage;
			}
		}

		public void OnInteract()
		{

		}

		private void OnNotInteractible(bool btnIsInteractible)
		{
			if (!btnIsInteractible)
			{
				_imageElement.color = new Color(_imageElement.color.r, _imageElement.color.r, _imageElement.color.b, 0.3f);
			}
			else
			{
				_imageElement.color = new Color(_imageElement.color.r, _imageElement.color.r, _imageElement.color.b, 1f);
			}
		}
	}
}
