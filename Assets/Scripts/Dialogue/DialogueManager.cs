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
        [SerializeField] TMP_Text tmpText;

        // public:
        public string[] dialogueBlock;
        public int dialogueIndex = 0;

        // private:
        private bool _currentlyInDialogue = false;
        private bool _nextDialogue = false;
        private bool _canExit = false;

        private void Start()
        {
            Instance ??= this;
        }

        void OnEnable()
        {
            TypewriterEffect.CompleteTextRevealed += FinishDialogue;
            //_dialogueUI.dialogueText.onDialogueFinish.AddListener(() => FinishDialogue());
        }

        private void OnDisable()
        {
            TypewriterEffect.CompleteTextRevealed -= FinishDialogue;
        }

        //Started by PlayerManager when an interactible has dialogue
        public void RunDialogue(string speakerID)
        {
            tmpText.maxVisibleCharacters = 0;

            if (!_currentlyInDialogue)
            {
                RetrieveDataFromDB(speakerID);
                _currentlyInDialogue = true;

                // UI
                _dialogueUI.FadeUI(true, .25f, .025f);

                // Write out text
                tmpText.text = dialogueBlock[0];

                //targetGroup.m_Targets[1].target = otherSpeaker;
            }
            else if (_nextDialogue)
            {
                tmpText.text = dialogueBlock[dialogueIndex];
            }
            else if (_canExit)
            {
                //UI
                _dialogueUI.FadeUI(false, .2f, .05f);

                GameStateManager.Instance.SetState(GameState.Freeroam);
                _currentlyInDialogue = false;
                _canExit = false;
            }

        }

        void RetrieveDataFromDB(string speakerID)
        {
            string dbConnectionPath = "URI=file:" + Application.dataPath + "/Data/Dialogue/DB_Debug-scene.db";

            // Connect to the SQLite database
            IDbConnection dbDialogue = new SqliteConnection(dbConnectionPath);

            // Open the database connection and create a command to execute SQL queries
            dbDialogue.Open();
            IDbCommand dbCommand = dbDialogue.CreateCommand();

            // Select all data from row specified by {keyvalue}
            dbCommand.CommandText = $"SELECT DISTINCT char, choices, mood, english FROM {ChooseTable()} WHERE speakID = {speakerID}";

            // Execute the query and retrieve the result
            IDataReader reader = dbCommand.ExecuteReader();
            dialogueBlock = CollectDiaogueIntoArray(reader);

            // Close the connections
            reader.Close();
            dbCommand.Dispose();
            dbDialogue.Close();

            //Change the table based on which Scene/Scenario is active;
            string ChooseTable()
            {
                return "dialogue01";
            }

            //Convert all the dialogue into an array
            string[] CollectDiaogueIntoArray(IDataReader reader)
            {
                List<string> stringList = new List<string>();

                // Check if there are rows in the result
                while (reader.Read())
                {
                    string value = reader.GetString(3); // Get all the dialogue from 4th column
                    stringList.Add(value);
                }

                string[] dialogueArray = stringList.ToArray();
                return dialogueArray;
            }
        }

        //Triggered when _textbox finish typing out text
        public void FinishDialogue()
        {
            if (dialogueIndex < dialogueBlock.Length - 1)
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