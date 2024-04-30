using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Arcy.Management;
using Arcy.Quests;
using Mono.Data.Sqlite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public event Action Skip;

        /// <summary>
        /// This script retrieves the data from the db and sends it to DialgueUI.
        /// But shuld be able to finish alone, without the support of DialogueUI. 
        /// </summary>

        [Header("Config")]
        [SerializeField] private bool _debugging = false;

        [Header("Dialogue UI")]
        [SerializeField] private DialogueUI _dialogueUI;
        [SerializeField] private TMP_Text _dialogueTMP;

        // public:
        [SerializeField] public LanguageEnum language = LanguageEnum.english; // Remember: Set up language handler
        [SerializeField] private int _dialogueIndex = 0;
        [Header("Other Speaker")]
        [SerializeField] public Transform otherSpeakerTransform;

        // private:
        [SerializeField] private int _speakerID = 1001;
        [SerializeField] private List<string> _dialogueBlock;
        [SerializeField] private List<string> _choices;
        [SerializeField] private List<string> _moods;

        private bool _newDialogueStarted = true;
        private bool _currentlyInDialogueBool = false;
        private bool _choiceBool = false;
        private bool _nextDialogueBool = false;
        private bool _canExitBool = false;

        private WaitForSeconds _delayBeforeDialogue;

        // Input started by PlayerManager when an interactible has dialogue
        public void RunDialogue(int speakerID)
        {
            _dialogueTMP.maxVisibleCharacters = 0;
            _dialogueUI.EnableDialogueBtns(false, true); // Hide all dialogue Btns

            // Choice is handled by AnswrBtnPressed
            if (_choiceBool)
            {
                return;
            }

            // Runs when a new dialogue plays up or we answer a question
            if (!_currentlyInDialogueBool)
            {
                _currentlyInDialogueBool = true;
                _speakerID = speakerID;
                _dialogueIndex = 0;
                _dialogueBlock = RetrieveDataFromDB(speakerID); // Retrieve dialogue from .db-file

                if (_dialogueUI.cvGroup.alpha != 1 || _newDialogueStarted)
                    _dialogueUI.FadeDialogueUI(true, .25f, .025f); // Fade in UI

                otherSpeakerTransform = Player.PlayerManager.instance.currentInteractible.ObjectTransform;

                StartCoroutine(ShortDelay());

                IEnumerator ShortDelay()
                {
                    if (_newDialogueStarted)
                        yield return _delayBeforeDialogue;
                    else
                        yield return null;

                    _dialogueTMP.text = _dialogueBlock[0]; // Write out text.
                }

                return;
            }
            else if (_nextDialogueBool) // Happens once we finish typing out the sentence
            {
                _nextDialogueBool = false;
                _dialogueTMP.text = _dialogueBlock[_dialogueIndex];

                return;
            }
            else if (_canExitBool) // What happens when we reach the end of the dialogue (and there's no question being asked).
            {
                // fade out UI
                _dialogueUI.FadeDialogueUI(false, .2f, .05f);

                _canExitBool = false;

                GameManager.instance.gameEventManager.dialogueEvents.DialogueFinished(_speakerID);

                // Reset all components
                _dialogueBlock.Clear();
                _choices.Clear();
                _moods.Clear();
                _speakerID = 0;
                otherSpeakerTransform = null;
                _dialogueTMP.text = null;
                _dialogueIndex = 0;
                _currentlyInDialogueBool = false;
                _newDialogueStarted = false;
                GameManager.instance.gameStateManager.SetState(GameState.Freeroam);
                return;
            }
            else if (_dialogueTMP.maxVisibleCharacters != _dialogueTMP.textInfo.characterCount - 1)
            {
                Skip?.Invoke();
                return;
            }
        }

        // Triggered when TypeWriterEffect finishes typing out text
        public void FinishTyping()
        {
            if (_currentlyInDialogueBool)
            {
                // Check whether there is a question being asked
                if (_choices[_dialogueIndex] != "null")
                {
                    _choiceBool = true;
                    _dialogueUI?.EnableDialogueBtns(true);
                    return;
                }

                if (_dialogueIndex < _dialogueBlock.Count - 1)
                {
                    _dialogueIndex++;
                    _nextDialogueBool = true;
                    _dialogueUI?.EnableDialogueBtns(false, false); // Only Show _nxtBtn
                }
                else
                {
                    _choiceBool = false;
                    _nextDialogueBool = false;
                    _canExitBool = true;
                    _dialogueUI?.EnableDialogueBtns(false, false); // Hide all dialogueBtns
                }
            }
        }

        // MARK: PRIVATE

        private void Awake()
        {
            // Set up AnswerBtns
            _dialogueUI.answrBtns[0].GetComponent<Button>().onClick.AddListener(YesButtonPressed);
            _dialogueUI.answrBtns[1].GetComponent<Button>().onClick.AddListener(NoButtonPressed);

            _delayBeforeDialogue = new WaitForSeconds(0.5f);
        }

        private void OnEnable()
        {
            TypewriterEffect.FinishTyping += FinishTyping;
        }

        private void OnDisable()
        {
            TypewriterEffect.FinishTyping -= FinishTyping;
        }

        // MARK: SQL-query
        private List<string> RetrieveDataFromDB(int speakerID)
        {
            string dbConnectionPath = $"URI=file:{Application.dataPath}/Data/Dialogue/DB_Debug-scene.db";

            // Connect to the SQLite database
            using (IDbConnection dbConnection = new SqliteConnection(dbConnectionPath))
            {
                try
                {
                    dbConnection.Open();

                    // Open the database connection and create a command to execute SQL queries
                    using (IDbCommand dbCommand = dbConnection.CreateCommand())
                    {
                        //Change the table based on which Scene/Scenario is active;
                        string dialogueTable = "dialogue01";

                        // Select all data from row specified by {keyvalue}
                        dbCommand.CommandText = $"SELECT DISTINCT choices, mood, {language} FROM {dialogueTable} WHERE speakID LIKE '{speakerID}%'";

                        // Execute the query and retrieve the result
                        IDataReader reader = dbCommand.ExecuteReader();

                        // Reset all the lists
                        List<string> dialogueList = new List<string>();
                        List<string> moods = new List<string>();
                        List<string> choices = new List<string>();

                        // Check if there are rows in the result
                        while (reader.Read())
                        {
                            string choiceColumn = reader["choices"].ToString();
                            string dialogue = reader["english"].ToString();
                            string moodColumn = reader["mood"].ToString();

                            dialogueList.Add(dialogue);

                            // Check choices
                            if (!string.IsNullOrEmpty(moodColumn))
                                moods.Add(moodColumn);
                            else
                                moods.Add(null);

                            // Check moods
                            if (!string.IsNullOrEmpty(choiceColumn))
                                choices.Add(choiceColumn);
                            else
                                choices.Add(null);
                        }

                        // Close the connections
                        reader.Close();
                        dbCommand.Dispose();

                        _moods = moods;
                        _choices = choices;

                        if (_debugging) Debug.Log(dialogueList[0] + " retrieved from DB.");

                        return dialogueList;
                    }
                }
                catch
                {
                    Debug.LogError("Unable to load a dialogueID from DB.");
                    return null;
                }
            }
        }

        // When we push one of the AnswrBtns
        private void AnswrBtnPressed(bool yesBtn)
        {
            StartCoroutine(ShortDelayBeforeExecution());

            IEnumerator ShortDelayBeforeExecution()
            {
                yield return null;

                if (yesBtn) // Yes-btn pressed
                    _speakerID = _speakerID + 1;
                else // No-btn pressed
                    _speakerID = _speakerID + 2;

                _choiceBool = false;
                _currentlyInDialogueBool = false;
                _newDialogueStarted = false;
                RunDialogue(_speakerID); // Launch a new conversation, based on a new _speakerID
            }
        }

        // When Yes-button is pressed
        private void YesButtonPressed()
        {
            AnswrBtnPressed(true);
        }

        // When Yes-button is pressed
        private void NoButtonPressed()
        {
            AnswrBtnPressed(false);
        }
    }
}