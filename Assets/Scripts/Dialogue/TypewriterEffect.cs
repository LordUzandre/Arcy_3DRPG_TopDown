using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Object = UnityEngine.Object;

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
		private bool _readyForNewText = true;

		private WaitForSeconds _simpleDelay;
		private WaitForSeconds _interpunctuationDelay;

		// Typewriter Settings
		private float _characterPerSecond = 40;
		private float _interpunctionationDelay = 0.2f;

		// Skipping Functionality
		public bool CurrentlySkipping { get; private set; }
		private WaitForSeconds _skipDelay;

		// Skippping Options
		private bool quickSkip;
		[Min(1)] private int skipSpeedUp = 5;

		// Event Functionality
		private WaitForSeconds _textBoxFullEventDelay;
		private float _sendDoneDelay = 0.25f;

		public static event Action CompleteTextRevealed;
		public static event Action<char> CharacterRevealed;

#if UNITY_EDITOR
		private void OnValidate()
		{
			_textBox ??= TryGetComponent<TMP_Text>(out TMP_Text hit) ? hit : null;
		}
#endif

		private void OnEnable()
		{
			_textBox ??= TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI textMesh) ? textMesh : null;

			_simpleDelay = new WaitForSeconds(1 / _characterPerSecond);
			_interpunctuationDelay = new WaitForSeconds(_interpunctionationDelay);
			_skipDelay = new WaitForSeconds(1 / _characterPerSecond * skipSpeedUp);
			_textBoxFullEventDelay = new WaitForSeconds(_sendDoneDelay);
			_textBox.maxVisibleCharacters = 0;

			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareForNewText); // Detects changes in ANY TMP-object in the scene

			//TODO: Create a subscription to inputManager for Skip().
		}

		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareForNewText);
		}

		private void PrepareForNewText(Object obj)
		{
			if (obj != _textBox || !_readyForNewText || _textBox.maxVisibleCharacters >= _textBox.textInfo.characterCount)
				return;

			CurrentlySkipping = false;
			_readyForNewText = false;

			if (_typewriterCoroutine != null)
				StopCoroutine(_typewriterCoroutine);

			_textBox.maxVisibleCharacters = 0; // use maxVisibleCharacters instead of .text to avoid string interpolation
			_currentlyVisibleCharacterIndex = 0;

			_typewriterCoroutine = StartCoroutine(routine: TypeWriter());

			IEnumerator TypeWriter()
			{
				TMP_TextInfo textInfo = _textBox.textInfo;

				while (_currentlyVisibleCharacterIndex < textInfo.characterCount + 1)
				{
					int lastCharacterIndex = textInfo.characterCount - 1;

					if (_currentlyVisibleCharacterIndex == lastCharacterIndex) // When we reach the final character in the text
					{
						_textBox.maxVisibleCharacters++;
						yield return _textBoxFullEventDelay; // Slight delay after typing has been finished

						CompleteTextRevealed?.Invoke();
						_readyForNewText = true;
						yield break;
					}

					char character = textInfo.characterInfo[_currentlyVisibleCharacterIndex].character;
					_textBox.maxVisibleCharacters++;

					if (!CurrentlySkipping && (character == '?' || character == '.' || character == ':' || character == ';' || character == '!'))
						yield return _interpunctuationDelay; //Slight delay at the end of a sentence.
					else
						yield return CurrentlySkipping ? _skipDelay : _simpleDelay; // regular delay based charactersPerSecond

					CharacterRevealed?.Invoke(character);
					_currentlyVisibleCharacterIndex++;
				}

			}
		}

		private void Skip() // Should be triggered by InputManager
		{
			if (CurrentlySkipping)
				return;

			CurrentlySkipping = true;

			if (!quickSkip)
			{
				StartCoroutine(routine: SkipSpeedupReset());
				return;
			}

			StopCoroutine(_typewriterCoroutine);
			_textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
			_readyForNewText = true;
			CompleteTextRevealed?.Invoke();
		}

		private IEnumerator SkipSpeedupReset()
		{
			yield return new WaitUntil(() => _textBox.maxVisibleCharacters == _textBox.textInfo.characterCount - 1);
			CurrentlySkipping = false;
		}
	}
}
