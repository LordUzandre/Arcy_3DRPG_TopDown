using System.Collections;
using System.Collections.Generic;
using Arcy.Dialogue;
using UnityEditor;
using UnityEngine;

namespace Arcy.Dialogue
{
    public interface ISpeakable
    {
        string SpeakerName { get; set; }
        DialogueBlock[] Dialogue { get; set; }
    }

    [System.Serializable]
    public class DialogueBlock
    {
        // public string dialogueID = "1001";
        public int dialogueID = 1001;
        public bool questRelated = false;
        public int questGUID;
        public Quests.QuestObjectiveEnum questStatus;

        public string GetDialogueID()
        {
            return dialogueID.ToString();
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DialogueBlock))]
    public class DialogueBlockEditor : PropertyDrawer
    {
        private SerializedProperty _dialogueID; // string
        private SerializedProperty _questRelated; // bool
        private SerializedProperty _questID; // string
        private SerializedProperty _label;

        private string _dialoguePreview = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, "
        + "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. "
        + "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";

        // PUBLIC:

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _dialogueID = property.FindPropertyRelative("dialogueID");
            _questRelated = property.FindPropertyRelative("questRelated");
            _questID = property.FindPropertyRelative("questGUID");

            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.LabelField(new Rect(), "DialogueBlock");
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginProperty(position, label, property);
            EditorGUILayout.PropertyField(_dialogueID); // int
            EditorGUILayout.PropertyField(_questRelated); // bool
            EditorGUI.indentLevel = 1;

            if (_questRelated.boolValue == true)
            {
                EditorGUILayout.PropertyField(_questID);
                property.isExpanded = true;
            }

            // EditorGUILayout.LabelField(_dialoguePreview, EditorStyles.whiteMiniLabel, GUILayout.ExpandHeight(true));
            EditorGUI.indentLevel++;
            EditorGUILayout.HelpBox(_dialoguePreview, MessageType.None);

            EditorGUI.indentLevel = 0;
            EditorGUI.EndProperty();
        }

    }
#endif

}