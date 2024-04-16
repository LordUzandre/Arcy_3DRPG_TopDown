using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Object = UnityEngine.Object;
using Arcy.Management;

namespace Arcy.Dialogue
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TypewriterEffect : MonoBehaviour
	{
		/// <summary>
		/// This script should be attached to any TMP that is going to be written out Typewriter-style.
		/// </summary>

		[SerializeField] private TMP_Text _textBox;

		//Basic typewriter functionality
		private int _currentlyVisibleCharacterIndex;
		private Coroutine _typewriterCoroutine;
		[SerializeField] public bool readyForNewText = true;

		private WaitForSeconds _simpleDelay;
		private WaitForSeconds _interpunctuationDelay;

		// Typewriter Settings
		private float _characterPerSecond = 50;
		private float _interpunctionationDelay = 0.2f;

		// Event Functionality
		private WaitForSeconds _textBoxFullEventDelay;
		private float _sendDoneDelay = 0.25f;

		public static event Action FinishTyping;
		// public static event Action<char> CharacterRevealed;

#if UNITY_EDITOR
		private void OnValidate()
		{
			_textBox ??= TryGetComponent<TMP_Text>(out TMP_Text hit) ? hit : null;
		}
#endif

		private void OnEnable()
		{
			_textBox ??= TryGetComponent<TMP_Text>(out TMP_Text textMesh) ? textMesh : null;

			_simpleDelay = new WaitForSeconds(1 / _characterPerSecond);
			_interpunctuationDelay = new WaitForSeconds(_interpunctionationDelay);
			_textBoxFullEventDelay = new WaitForSeconds(_sendDoneDelay);
			_textBox.maxVisibleCharacters = 0;

			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareForNewText); // Detects changes in ANY TMP-object in the scene
			GameManager.instance.gameEventManager.dialogueEvents.onSkipTyping += Skip;
		}

		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareForNewText);
			GameManager.instance.gameEventManager.dialogueEvents.onSkipTyping -= Skip;
		}

		// When the text in _textbox is changed by another script, this types it out.
		private void PrepareForNewText(Object obj)
		{
			if (obj != _textBox || !readyForNewText || _textBox.maxVisibleCharacters >= _textBox.textInfo.characterCount)
				return;

			readyForNewText = false;

			if (_typewriterCoroutine != null)
				StopCoroutine(_typewriterCoroutine);

			_textBox.maxVisibleCharacters = 0; // use .maxVisibleCharacters instead of .text to avoid string interpolation
			_currentlyVisibleCharacterIndex = 0;

			_typewriterCoroutine = StartCoroutine(routine: TypeWriter()); // Use reference so that it is easier to stop.

			IEnumerator TypeWriter()
			{
				TMP_TextInfo textInfo = _textBox.textInfo; // TextInfo contains the full text that's going to be printed out

				while (_currentlyVisibleCharacterIndex < textInfo.characterCount + 1)
				{
					int lastCharacterIndex = textInfo.characterCount - 1;

					if (_currentlyVisibleCharacterIndex == lastCharacterIndex) // When we reach the final character in the text
					{
						_textBox.maxVisibleCharacters++;
						yield return _textBoxFullEventDelay; // Slight delay after typing has been finished

						FinishTyping?.Invoke();
						readyForNewText = true;
						yield break;
					}

					char character = textInfo.characterInfo[_currentlyVisibleCharacterIndex].character;
					_textBox.maxVisibleCharacters++;

					if (character == '?' || character == '.' || character == ':' || character == ';' || character == '!')
						yield return _interpunctuationDelay; //Slight delay at the end of a sentence.
					else
						yield return _simpleDelay;

					// CharacterRevealed?.Invoke(character); // Use if we want to keep tabs of the dialogue ex. count punctioation marks
					_currentlyVisibleCharacterIndex++;
				}

			}
		}

		public void Skip() // Triggered by dialogueManager
		{
			if (_typewriterCoroutine != null)
			{
				StopCoroutine(_typewriterCoroutine);
			}
			_textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
			readyForNewText = true;
			FinishTyping?.Invoke();
		}
	}
}
