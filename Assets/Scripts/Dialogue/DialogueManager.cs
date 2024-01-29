using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcy.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        //singleton
        private static DialogueManager instance;
        public static DialogueManager Instance { get; private set; }

        public event Action Skip;

        /// <summary>
        /// This script retrieves the data from the db and sends it to DialgueUI.
        /// But shuld be able to finish alone, without the support of DialogueUI. 
        /// </summary>

        [Header("Dialogue UI")]
        [SerializeField] private DialogueUI _dialogueUI;
        [SerializeField] private TMP_Text _tmpText;

        // public:
        public int _dialogueIndex = 0;
        [SerializeField] private string _speakerID;
        public string language = "english"; // Remember: Set up language handler

        // private:
        [SerializeField] private List<string> _dialogueBlock;
        [SerializeField] private List<string> _choices;
        [SerializeField] private List<string> _moods;

        [SerializeField] private bool _currentlyInDialogue = false;
        [SerializeField] private bool _choice = false;
        private bool _nextDialogue = false;
        private bool _canExit = false;

        private WaitForSeconds _delayBeforeDialogue;

        [Header("Other Speaker")]
        [SerializeField] public Transform otherSpeakerTransform; // TODO: Should be other object's eyes, not just their transform.

        private void Start()
        {
            Instance ??= this;

            // Set up AnswerBtns
            _dialogueUI.answrBtns[0].GetComponent<Button>().onClick.AddListener(YesButtonPressed);
            _dialogueUI.answrBtns[1].GetComponent<Button>().onClick.AddListener(NoButtonPressed);
        }

        private void OnEnable()
        {
            TypewriterEffect.FinishTyping += FinishTyping;

            //_shortDelay = new WaitForSeconds(0.05f);
            _delayBeforeDialogue = new WaitForSeconds(0.5f);
        }

        private void OnDisable()
        {
            TypewriterEffect.FinishTyping -= FinishTyping;
        }

        #region Input from InputManager
        // Input started by PlayerManager when an interactible has dialogue
        public void RunDialogue(string speakerID)
        {
            _tmpText.maxVisibleCharacters = 0;
            _dialogueUI.EnableDialogueBtns(false, true); // Hide all dialogue Btns

            if (_choice) // Choice is handled by AnswrBtnPressed
            {
                Debug.Log("_choice is blocking");
                _choice = false;
                return;
            }

            if (!_currentlyInDialogue) // Runs when a new dialogue plays up
            {
                _currentlyInDialogue = true;
                _speakerID = speakerID;
                _dialogueIndex = 0;
                _dialogueBlock = RetrieveDataFromDB(speakerID); // Retrieve dialogue from .db-file

                if (_dialogueUI.cvGroup.alpha != 1)
                    _dialogueUI.FadeDialogueUI(true, .25f, .025f); // Fade in UI

                StartCoroutine(ShortDelay());

                IEnumerator ShortDelay()
                {
                    yield return _delayBeforeDialogue;
                    _tmpText.text = _dialogueBlock[0]; // Write out text.
                }

                //targetGroup.m_Targets[1].target = otherSpeaker;

                return;
            }
            else if (_nextDialogue) // What happens once we finish typing out the sentence
            {
                _nextDialogue = false;
                _tmpText.text = _dialogueBlock[_dialogueIndex];

                return;
            }
            else if (_canExit) // What happens when we reach the end of the dialogue, and there's no question being asked.
            {
                _dialogueUI.FadeDialogueUI(false, .2f, .05f); // fade out UI

                _canExit = false;

                // Reset TMP
                _dialogueBlock.Clear();
                _choices.Clear();
                _moods.Clear();
                _speakerID = null;
                _tmpText.text = null;
                _dialogueIndex = 0;
                _currentlyInDialogue = false;

                GameStateManager.Instance.SetState(GameState.Freeroam);

                return;
            }
            else if (_tmpText.maxVisibleCharacters != _tmpText.textInfo.characterCount - 1)
            {
                // TODO! For some reason this runs whenever we answer a question
                Skip?.Invoke();
                Debug.Log("Skip triggered");
                return;
            }
        }
        #endregion

        #region SQL-query
        List<string> RetrieveDataFromDB(string speakerID)
        {
            string dbConnectionPath = "URI=file:" + Application.dataPath + "/Data/Dialogue/DB_Debug-scene.db";

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
        #endregion

        // Triggered when TypeWriterEffect finishes typing out text
        public void FinishTyping()
        {
            if (_currentlyInDialogue)
            {
                // Check whether there is a question being asked
                if (_choices[_dialogueIndex] != "null")
                {
                    _choice = true;
                    _dialogueUI?.EnableDialogueBtns(true);
                    return;
                }

                if (_dialogueIndex < _dialogueBlock.Count - 1)
                {
                    _dialogueIndex++;
                    _nextDialogue = true;
                    _dialogueUI?.EnableDialogueBtns(false, false); // Only Show _nxtBtn
                }
                else
                {
                    _choice = false;
                    _nextDialogue = false;
                    _canExit = true;
                    _dialogueUI?.EnableDialogueBtns(false, false); // Hide all dialogueBtns
                }
            }
        }

        // When we push one of the AnswrBtns
        private void AnswrBtnPressed(bool yesBtn)
        {
            // _dialogueUI.EnableDialogueBtns(false, false);
            _tmpText.text = "";
            _choice = false;

            if (yesBtn) // Yes-btn pressed
                _speakerID = AnswerFromBtn(_speakerID, 1);
            else // No-btn pressed
                _speakerID = AnswerFromBtn(_speakerID, 2);

            // Create a new string based on which AnswrBtn is pressed
            string AnswerFromBtn(string ogString, int addValue)
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

            _currentlyInDialogue = false;

            RunDialogue(_speakerID); // Launch a new conversation, based on a new _speakerID
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

        /*
        // TODO: All methods below should be put in CameraManager
        public void CameraChange(bool dialogue) //true = dialogue, false = freeroam
        {
            if (dialogueCam != null)
            {
                gameCam.SetActive(!dialogue);
                dialogueCam.SetActive(dialogue);
            }

            //Depth of field modifier
            if (dialogueDof != null)
            {
                float dofWeight = dialogueCam.activeSelf ? 1 : 0;
                DOVirtual.Float(dialogueDof.weight, dofWeight, .8f, DialogueDOF);
            }
        }

        public void DialogueDOF(float x)
        {
            dialogueDof.weight = x;
        }ws
        */
    }

}