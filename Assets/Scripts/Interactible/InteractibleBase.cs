using System;
using UnityEngine;

namespace Arcy.Interaction
{
    public abstract class InteractibleBase : MonoBehaviour
    {
        public enum IconType { SpeechBubble, QuestBubble, SignBubble, Triangle }
        //public:
        [Header("Speech Bubble")]
        [Space]
        [field: SerializeField] private UnityEngine.UI.Image[] bubbleSprites;
        private UnityEngine.UI.Image speechBubble;
        [field: SerializeField] IconType icontype; //enum

        [Header("SpeechBubble Indicator GameObject")]
        public GameObject speechObject;

        //Actions
        public static Action<Vector3> MoveIconHere;
        public static Action RemoveIcon;

        //private:

        private void Reset()
        {
            bubbleSprites = new UnityEngine.UI.Image[4];
        }

        private void OnEnable()
        {
            if (speechObject == null)
            {
                speechObject = GameObject.FindGameObjectWithTag("DialogueUI");
            }

            if (gameObject.tag == "Untagged")
            {
                gameObject.tag = "Interactible";
            }
        }

        public virtual void myMethod()
        {
            int bubbleIndex = 0;

            switch (icontype)
            {
                case (IconType.SpeechBubble):
                    bubbleIndex = 0;
                    break;
                case (IconType.QuestBubble):
                    bubbleIndex = 1;
                    break;
                case (IconType.SignBubble):
                    bubbleIndex = 2;
                    break;
                case (IconType.Triangle):
                    bubbleIndex = 3;
                    break;
                default:
                    break;
            }

            speechBubble = bubbleSprites[bubbleIndex];
        }

        //When the interactible is within Player's fow, triggered by fieldOfView.cs
        public void OnFocus()
        {
            speechObject.transform.position = this.transform.position;
        }

        //When the interactible is out of range for player, triggered by FieldOfView.cs
        public void OnDefocused()
        {
            speechObject.transform.position = this.transform.position;
        }
    }
}
