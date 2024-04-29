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

        [Header("Dialogue UI")]
        [SerializeField] private DialogueUI _dialogueUI;
        [SerializeField] private TMP_Text _dialogueTMP;

        // public:
        public LanguageEnum language = LanguageEnum.english; // Remember: Set up language handler
        public int _dialogueIndex = 0;

        // private:
        [SerializeField] private string _speakerID;
        [SerializeField] private List<string> _dialogueBlock;
        [SerializeField] private List<string> _choices;
        [SerializeField] private List<string> _moods;

        private bool _newDialogueStarted = true;
        private bool _currentlyInDialogueBool = false;
        private bool _choiceBool = false;
        private bool _nextDialogueBool = false;
        private bool _canExitBool = false;

        private WaitForSeconds _delayBeforeDialogue;

        [Header("Other Speaker")]
        [SerializeField] public Transform otherSpeakerTransform;

        // MARK: Input from InputManager

        public string GetAppropriateDialogueString(DialogueBlock[] dialogueArray)
        {
            string nonQuestRelatedDialogue = "1001";
            string mostRecentQuestDialogue = "1002";

            QuestManager questManager = transform.Find("QuestManager").GetComponent<QuestManager>();

            foreach (DialogueBlock dialogue in dialogueArray)
            {
                if (!dialogue.questRelated && nonQuestRelatedDialogue == "1001")
                {
                    nonQuestRelatedDialogue = dialogue.dialogueID;
                    break;
                }

                // Search through Quest-Log to see most recent quest.
                foreach (Quest quest in questManager.questLog.Values)
                {
                    Quest thisQuest = questManager.GetQuestByGuid(dialogue.questGUID);

                    if (thisQuest != null)
                    {
                        if (quest.currentStatusEnum == dialogue.questStatus)
                        {
                            mostRecentQuestDialogue = dialogue.dialogueID;
                        }

                        return mostRecentQuestDialogue;
                    }
                }
            }

            return nonQuestRelatedDialogue;
        }

        // Input started by PlayerManager when an interactible has dialogue
        public void RunDialogue(string speakerID)
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
                _speakerID = null;
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

        private void Start()
        {
            // Set up AnswerBtns
            _dialogueUI.answrBtns[0].GetComponent<Button>().onClick.AddListener(YesButtonPressed);
            _dialogueUI.answrBtns[1].GetComponent<Button>().onClick.AddListener(NoButtonPressed);
        }

        private void OnEnable()
        {
            TypewriterEffect.FinishTyping += FinishTyping;

            _delayBeforeDialogue = new WaitForSeconds(0.5f);
        }

        private void OnDisable()
        {
            TypewriterEffect.FinishTyping -= FinishTyping;
        }

        // MARK: SQL-query
        private List<string> RetrieveDataFromDB(string speakerID)
        {
            string dbConnectionPath = $"URI=file:{Application.dataPath}/Data/Dialogue/DB_Debug-scene.db";

            // Connect to the SQLite database
            IDbConnection dbDialogue = new SqliteConnection(dbConnectionPath);

            // Open the database connection and create a command to execute SQL queries
            dbDialogue.Open();
            IDbCommand dbCommand = dbDialogue.CreateCommand();

            //Change the table based on which Scene/Scenario is active;
            string chooseTable = "dialogue01";

            // Select all data from row specified by {keyvalue}
            dbCommand.CommandText = $"SELECT DISTINCT choices, mood, {language} FROM {chooseTable} WHERE speakID LIKE '{speakerID}%'";

            // Execute the query and retrieve the result
            IDataReader reader = dbCommand.ExecuteReader();

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
                    moods.Add("null");

                // Check moods
                if (!string.IsNullOrEmpty(choiceColumn))
                    choices.Add(choiceColumn);
                else
                    choices.Add("null");
            }

            // Close the connections
            reader.Close();
            dbCommand.Dispose();
            dbDialogue.Close();

            _moods = moods;
            _choices = choices;

            return dialogueList;
        }

        // When we push one of the AnswrBtns
        private void AnswrBtnPressed(bool yesBtn)
        {
            StartCoroutine(ShortDelayBeforeExecution());

            IEnumerator ShortDelayBeforeExecution()
            {
                if (yesBtn) // Yes-btn pressed
                    _speakerID = GetNewSpeakerId(_speakerID, 1);
                else // No-btn pressed
                    _speakerID = GetNewSpeakerId(_speakerID, 2);

                // Create a new string based on which AnswrBtn is pressed
                string GetNewSpeakerId(string ogString, int addValue)
                {
                    string numericPart = "";
                    string updatedString = "";

                    // Extract numeric part from the string
                    foreach (char charLetter in ogString)
                    {
                        if (char.IsDigit(charLetter))
                            numericPart += charLetter;
                        else
                            break; // Stop when a non-digit character is encountered
                    }

                    // Convert numeric part to an integer
                    if (int.TryParse(numericPart, out int numericValue))
                    {
                        // Perform the desired operation on the numeric value
                        numericValue += addValue;

                        // Convert the updated numeric value back to string and append the non-numeric part
                        updatedString = numericValue.ToString() + ogString.Substring(numericPart.Length);
                    }

                    return updatedString;
                }

                yield return null;

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