using System.Collections;
using System.Collections.Generic;
using Arcy.Dialogue;
using UnityEditor;
using UnityEngine;

namespace Arcy.Interaction
{
    public interface ISpeakable
    {
        string SpeakerName { get; set; }
        DialogueBlock[] Dialogue { get; set; }
    }

    [System.Serializable]
    public class DialogueBlock
    {
        public string dialogueID;
        public bool questRelated = false;
        public int questGUID;
        public Quests.QuestObjectiveEnum questStatus;

        public string GetDialogueID()
        {
            return dialogueID;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DialogueBlock))]
    public class DialogueBlockEditor : PropertyDrawer
    {
        private SerializedProperty _dialogueID; // string
        private SerializedProperty _questRelated; // bool
        private SerializedProperty _questID; // string

        // private bool _questrelated;
        private string dialoguePreview = "Is it possible to see a preview of the dialogue here?";
        private int numberOfLines = 0;

        // PUBLIC:

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int rowNumber = 0;
            EditorGUI.BeginProperty(position, label, property);

            _dialogueID = property.FindPropertyRelative("dialogueID");
            _questRelated = property.FindPropertyRelative("questRelated");
            _questID = property.FindPropertyRelative("questGUID");

            EditorGUI.PropertyField(drawRect(position, rowNumber), _dialogueID, new GUIContent("Dialogue ID"));
            rowNumber++;
            EditorGUI.PropertyField(drawRect(position, rowNumber), _questRelated, new GUIContent("Dialogue is Quest Related"));
            rowNumber++;

            EditorGUI.indentLevel = 1;

            if (_questRelated.boolValue == true)
            {
                EditorGUI.PropertyField(drawRect(position, rowNumber), _questID, new GUIContent("Quest ID"));
                rowNumber++;
            }

            EditorGUI.indentLevel = 0;
            // EditorGUI.PrefixLabel(drawRect(position, rowNumber), new GUIContent("Preview of the Dialogue"));
            EditorGUILayout.HelpBox(dialoguePreview, MessageType.None);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            numberOfLines = 3;

            if (property.isArray == true)
            {
                numberOfLines = 5;
            }

            return EditorGUIUtility.singleLineHeight * numberOfLines;
        }

        // PRIVATE:

        private Rect drawRect(Rect position, int numberInList)
        {
            float xStartPos = position.min.x;
            float yStartPos = position.min.y + (EditorGUIUtility.singleLineHeight * numberInList) + (2 * numberInList);
            float width = position.size.x * 0.99f;
            float height = EditorGUIUtility.singleLineHeight;
            Rect rect = new Rect(xStartPos, yStartPos, width, height);
            return rect;
        }
    }
#endif
}