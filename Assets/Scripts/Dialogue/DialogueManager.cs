using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
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
            _dialogueUI.dialogueText.onDialogueFinish.AddListener(() => FinishDialogue());
        }

        //Started by PlayerManager when an interactible has dialogue
        public void RunDialogue(string speakerID)
        {
            if (!_currentlyInDialogue)
            {
                RetrieveDataFromDB(speakerID);
                _currentlyInDialogue = true;

                // UI
                _dialogueUI.FadeUI(true, .25f, .025f);

                //targetGroup.m_Targets[1].target = otherSpeaker;
            }
            else if (_nextDialogue)
            {
                // Run next line of dialogue
                _dialogueUI.TypeOutDialogueText(dialogueIndex);
            }
            else if (_canExit)
            {
                //UI
                _dialogueUI.FadeUI(false, .2f, .05f);

                GameStateManager.Instance.SetState(GameState.Freeroam);
                Invoke("ResetState", .8f);
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
        }

        //Change the table based on which Scene/Scenario is active;
        string ChooseTable()
        {
            return "dialogue01";
        }

        //Convert all the dialogue into an array
        private string[] CollectDiaogueIntoArray(IDataReader reader)
        {
            List<string> stringList = new List<string>();

            // Check if there are rows in the result
            while (reader.Read())
            {
                string value = reader.GetString(3);
                stringList.Add(value);
            }

            string[] dialogueArray = stringList.ToArray();
            return dialogueArray;
        }

        public void ResetState()
        {
            PlayerManager.instance.EnableMovement(true);
            _currentlyInDialogue = false;
            _canExit = false;
        }

        //Triggered by TMP_Animated
        public void FinishDialogue()
        {
            //_dialogueUI._currentlyTyping = false;

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