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

        /// <summary>
        /// This script retrieves the data from the db and sends it to DialgueUI.
        /// But shuld be able to finish alone, without the support of DialogueUI. 
        /// </summary>

        [Header("Dialogue UI")]
        [SerializeField] private DialogueUI _dialogueUI;
        [SerializeField] private TMP_Text _tmpText;

        // public:
        public int dialogueIndex = 0;

        // private:
        [SerializeField] private List<string> _dialogueBlock;
        [SerializeField] private List<string> _choices;
        [SerializeField] private List<string> _moods;
        private bool _currentlyInDialogue = false;
        private bool _nextDialogue = false;
        private bool _canExit = false;

        private WaitForSeconds _shortDelay;
        [SerializeField] private TypewriterEffect typeWriter;

        private void Start()
        {
            Instance ??= this;
        }

        void OnEnable()
        {
            TypewriterEffect.FinishTyping += FinishTyping;

            typeWriter ??= _tmpText.TryGetComponent<TypewriterEffect>(out TypewriterEffect typer) ? typer : null;
            _shortDelay = new WaitForSeconds(0.5f);
        }

        private void OnDisable()
        {
            TypewriterEffect.FinishTyping -= FinishTyping;
        }

        //Started by PlayerManager when an interactible has dialogue
        public void RunDialogue(string speakerID)
        {
            _tmpText.maxVisibleCharacters = 0;
            _dialogueUI.EnableDialogueBtns(false, true);

            if (!_currentlyInDialogue)
            {
                _currentlyInDialogue = true;

                // Retrieve dialogue from .db-file
                dialogueIndex = 0;
                _dialogueBlock = RetrieveDataFromDB(speakerID);

                // UI
                _dialogueUI.FadeDialogueUI(true, .25f, .025f);

                StartCoroutine(ShortDelay());

                IEnumerator ShortDelay()
                {
                    yield return _shortDelay;
                    _tmpText.text = _dialogueBlock[0]; // Write out text.
                }

                //targetGroup.m_Targets[1].target = otherSpeaker;
            }
            else if (_nextDialogue) // only available once we finish typing out the sentence
            {
                _nextDialogue = false;
                _tmpText.text = _dialogueBlock[dialogueIndex];
            }
            else if (_canExit)
            {
                _dialogueUI.FadeDialogueUI(false, .2f, .05f); //UI

                _canExit = false;

                // Reset TMP
                _dialogueBlock.Clear();
                _choices.Clear();
                _moods.Clear();

                _tmpText.text = "";
                dialogueIndex = 0;
                _currentlyInDialogue = false;

                GameStateManager.Instance.SetState(GameState.Freeroam);
            }
            else if (_tmpText.maxVisibleCharacters != _tmpText.textInfo.characterCount - 1)
            {
                typeWriter.Skip();
                // Works for the first sentence, but not the second.
            }
        }

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
            dbCommand.CommandText = $"SELECT DISTINCT choices, mood, english FROM {chooseTable} WHERE speakID LIKE '{speakerID}%'";

            // Execute the query and retrieve the result
            IDataReader reader = dbCommand.ExecuteReader();

            List<string> dialogueList = new List<string>();
            List<string> moods = new List<string>();
            List<string> choices = new List<string>();

            // Check if there are rows in the result
            while (reader.Read())
            {
                string choiceColumn = reader["choices"].ToString();
                string dialogue = reader.GetString(2); // Get all the dialogue from 3rd column
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

        //Triggered when _textbox finish typing out text
        public void FinishTyping()
        {
            if (_currentlyInDialogue)
            {
                // Check whether there is a question being asked
                if (_choices[dialogueIndex] != "null")
                {
                    _dialogueUI.EnableDialogueBtns(true);
                    SetUpAnswrBtns();
                    Debug.Log("Part01");
                    return;
                }

                if (dialogueIndex < _dialogueBlock.Count - 1)
                {
                    dialogueIndex++;
                    _nextDialogue = true;
                    _dialogueUI.EnableDialogueBtns(false, false);
                }
                else
                {
                    _nextDialogue = false;
                    _canExit = true;
                    _dialogueUI.EnableDialogueBtns(false, false);
                }
            }
        }

        private void SetUpAnswrBtns()
        {
            _dialogueUI.answrBtns[0].GetComponent<Button>().onClick.AddListener(YesBtnPressed);
            _dialogueUI.answrBtns[1].GetComponent<Button>().onClick.AddListener(NoBtnPressed);
        }

        private void YesBtnPressed()
        {
            Debug.Log("YesBtn Pressed");
        }

        private void NoBtnPressed()
        {
            Debug.Log("NoBtn Pressed");
        }


        /*
        // TODO: All methods below should be put in CameraManager
        */

        // public void CameraChange(bool dialogue) //true = dialogue, false = freeroam
        // {
        //     if (dialogueCam != null)
        //     {
        //         gameCam.SetActive(!dialogue);
        //         dialogueCam.SetActive(dialogue);
        //     }

        //     //Depth of field modifier
        //     if (dialogueDof != null)
        //     {
        //         float dofWeight = dialogueCam.activeSelf ? 1 : 0;
        //         DOVirtual.Float(dialogueDof.weight, dofWeight, .8f, DialogueDOF);
        //     }
        // }

        // public void DialogueDOF(float x)
        // {
        //     dialogueDof.weight = x;
        // }
    }

}