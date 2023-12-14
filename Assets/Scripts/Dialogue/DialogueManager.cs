using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;
using TMPro;
using Arcy.Interaction;

namespace Arcy.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        //singleton
        public static DialogueManager instance;

        //public:
        [Header("Dialogue Canvas Group")]
        [SerializeField]
        public CanvasGroup cvGroup;
        [Space]
        [Header("Cameras")]
        [SerializeField]
        public GameObject gameCam;
        [SerializeField]
        public GameObject dialogueCam;
        [SerializeField]
        public CinemachineTargetGroup targetGroup;
        [Space]
        [Header("Post-proccessing")]
        [SerializeField]
        public UnityEngine.Rendering.Volume dialogueDof;
        [Space]
        [Header("Dialogue Text")]
        [SerializeField]
        public TMP_Animated dialogueUIText;

        [HideInInspector]
        //public InteractibleBase currentInteractible;

        //private:
        private string[] dialogueBlock;
        private int _dialogueIndex = 0;
        private bool _currentlyInDialogue = false;
        private bool _nextDialogue = false;
        private bool _canExit = false;

        private enum LanguageSelection { english, spanish, swedish, klingon, rövarspråket, all_other_languages  };
        [SerializeField] private LanguageSelection choosenLanguage;

        private void Awake()
        {
            if (instance == null) { instance = this; } else { Destroy(this); }
        }

        private void Start()
        {
            if (cvGroup == null)
            {
                GameObject.FindGameObjectWithTag("DialogueUI").GetComponent<CanvasGroup>();
            }

            if (gameCam == null)
            {
                GameObject.FindGameObjectWithTag("MainCamera");
            }

            cvGroup.alpha = 0;
            dialogueUIText.onDialogueFinish.AddListener(() => FinishDialogue());
        }

        //Started by PlayerManager when there an interactible has dialogue
        public void RunDialogue(InteractibleBase currentInteractible, string speakerID, Transform otherSpeakerPosition, bool cameraShouldChange = false)
        {
            if (!_currentlyInDialogue)
            {
                StartDialogue(speakerID, otherSpeakerPosition);
            }
            else
            {
                if (_canExit)
                {
                    CameraChange(false);
                    FadeUI(false, .2f, .05f);
                    Sequence sequence = DOTween.Sequence();
                    sequence.AppendInterval(.8f);
                    sequence.AppendCallback(() => ResetState());
                }

                if (_nextDialogue)
                {
                    dialogueUIText.ReadText(dialogueBlock[_dialogueIndex]);
                    //dialogueUIText.ReadText(currentInteractible.dialogue.Sentences[_dialogueIndex]);
                }
            }
        }

        public void StartDialogue(string speakerID, Transform otherSpeaker)
        {
            //currentInteractible = PlayerManager.instance.currentInteractible;

            RetrieveDataFromDB(speakerID);

            // Change the camera
            targetGroup.m_Targets[1].target = otherSpeaker;
            //targetGroup.m_Targets[1].target = currentInteractible.gameObject.transform;

            // UI
            _currentlyInDialogue = true;
            ClearText();
            CameraChange(true);
            FadeUI(true, .25f, .025f);
        }

        public void FadeUI(bool show, float time, float delay)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(delay);
            sequence.Append(cvGroup.DOFade(show ? 1 : 0, time));

            if (show)
            {
                _dialogueIndex = 0;
                sequence.Join(cvGroup.transform.DOScale(0, time * 2).From().SetEase(Ease.OutBack));
                sequence.AppendCallback(() => dialogueUIText.ReadText(dialogueBlock[0]));
                //sequence.AppendCallback(() => dialogueUIText.ReadText(currentInteractible.dialogue.Sentences[0]));
            }
        }

        public void ClearText()
        {
            dialogueUIText.text = string.Empty;
        }

        public void ResetState()
        {
            PlayerManager.instance.EnableMovement(true);
            _currentlyInDialogue = false;
            _canExit = false;
        }

        public void FinishDialogue()
        {
            if (_dialogueIndex < dialogueBlock.Length - 1)
            {
                _dialogueIndex++;
                _nextDialogue = true;
            }
            else
            {
                _nextDialogue = false;
                _canExit = true;
            }
        }

        //Should be put in CameraManager
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

    }

}