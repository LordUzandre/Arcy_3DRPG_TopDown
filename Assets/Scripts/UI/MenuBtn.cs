using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Arcy.UI
{
	public class MenuBtn : MonoBehaviour
	{
		[Header("Non-selected images")]
		[SerializeField] private Sprite _disabledSprite;
		[SerializeField] private Sprite _neutralSprite;
		[SerializeField] private Sprite _highlightedSprite;

		[Header("selected system")]
		[SerializeField] private Sprite _highlightAndSelectedSprite;
		[SerializeField] private Sprite _selectedImageSprite;

		[Header("Explicit Navigation")]
		[SerializeField] public MenuBtn OnUpSelect;
		[SerializeField] public MenuBtn OnLeftSelect;
		[SerializeField] public MenuBtn OnRightSelect;
		[SerializeField] public MenuBtn OnDownSelect;

		[SerializeField] public bool isInteractible = true;
		[HideInInspector] public bool isHighlighted = false;

		[Space]
		[SerializeField] public UnityEvent onBtnClick;

		private Image _imageElement;
		private TMP_Text _textMeshPro;

		private void OnEnable()
		{
			_imageElement = TryGetComponent<Image>(out Image myImage) ? myImage : null;
			_textMeshPro = GetComponentInChildren<TMP_Text>();
		}

		private void OnValidate()
		{
			if (!isInteractible)
			{
				_imageElement.sprite = _disabledSprite;
			}
		}

		public void OnSelected()
		{
			if (!isHighlighted)
			{
				_imageElement.sprite = _selectedImageSprite;
			}
			else
			{
				_imageElement.sprite = _highlightAndSelectedSprite;
			}
		}

		public void OnHighlighted()
		{
			_imageElement.sprite = _highlightedSprite;
		}

		public void OnDeselected()
		{
			if (isHighlighted)
			{
				_imageElement.sprite = _highlightedSprite;
			}
			else if (isInteractible)
			{
				_imageElement.sprite = _neutralSprite;
			}
			else
			{
				_imageElement.sprite = _disabledSprite;
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
