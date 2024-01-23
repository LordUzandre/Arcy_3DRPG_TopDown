using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using TMPro;
using UnityEngine;

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
        [SerializeField] private int visibleChar; // Debug

        // public:
        public int dialogueIndex = 0;

        // private:
        [SerializeField] private List<string> _dialogueBlock;
        private bool _currentlyInDialogue = false;
        private bool _nextDialogue = false;
        private bool _canExit = false;

        private void Start()
        {
            Instance ??= this;
        }

        void OnEnable()
        {
            TypewriterEffect.FinishTyping += FinishTyping;
        }

        private void OnDisable()
        {
            TypewriterEffect.FinishTyping -= FinishTyping;
        }

        private void Update()
        {
            visibleChar = _tmpText.maxVisibleCharacters;
        }

        //Started by PlayerManager when an interactible has dialogue
        public void RunDialogue(string speakerID)
        {
            _tmpText.maxVisibleCharacters = 0;

            if (!_tmpText.gameObject.TryGetComponent<TypewriterEffect>(out TypewriterEffect hit))
            {
                if (!hit._readyForNewText)
                    return;
            }

            if (!_currentlyInDialogue)
            {
                _currentlyInDialogue = true;

                // Retrieve dialogue from .db-file
                dialogueIndex = 0;
                _dialogueBlock = RetrieveDataFromDB(speakerID);

                // UI
                _dialogueUI.FadeUI(true, .25f, .025f);

                _tmpText.text = _dialogueBlock[0]; // Write out text.

                //targetGroup.m_Targets[1].target = otherSpeaker;
            }
            else if (_nextDialogue) // only available once we finish typing out the sentence
            {
                _nextDialogue = false;
                _tmpText.text = _dialogueBlock[dialogueIndex];
            }
            else if (_canExit)
            {
                _dialogueUI.FadeUI(false, .2f, .05f); //UI

                _canExit = false;

                // Reset TMP
                _dialogueBlock.Clear();
                _tmpText.text = "";
                dialogueIndex = 0;
                //_tmpText.maxVisibleCharacters = 0;
                _currentlyInDialogue = false;

                GameStateManager.Instance.SetState(GameState.Freeroam);
            }
            else if (_tmpText.gameObject.TryGetComponent<TypewriterEffect>(out TypewriterEffect typewriterEffect))
            {
                if (_tmpText.maxVisibleCharacters != _tmpText.textInfo.characterCount - 1)
                {
                    typewriterEffect.Skip();
                    // Works for the first sentence, but not the second.
                }
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
            dbCommand.CommandText = $"SELECT DISTINCT char, choices, mood, english FROM {chooseTable} WHERE speakID = {speakerID}";

            // Execute the query and retrieve the result
            IDataReader reader = dbCommand.ExecuteReader();

            List<string> dialogueList = new List<string>();

            // Check if there are rows in the result
            while (reader.Read())
            {
                string value = reader.GetString(3); // Get all the dialogue from 4th column
                dialogueList.Add(value);
            }

            // Close the connections
            reader.Close();
            dbCommand.Dispose();
            dbDialogue.Close();

            return dialogueList;
        }

        //Triggered when _textbox finish typing out text
        public void FinishTyping()
        {
            if (_currentlyInDialogue)
            {
                if (dialogueIndex < _dialogueBlock.Count - 1)
                {
                    dialogueIndex++;
                    _nextDialogue = true;
                }
                else
                {
                    _nextDialogue = false;
                    _canExit = true;
                }
            }
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