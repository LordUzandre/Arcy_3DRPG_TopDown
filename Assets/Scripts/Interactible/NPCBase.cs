using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Animation;
using Arcy.Dialogue;
using Arcy.Battle;
using DG.Tweening;

namespace Arcy.Interaction
{
    public class NPCBase : MonoBehaviour, IInteractibleBase, ISpeakable
    {

        [SerializeField] string speakerName { get; set; }
        public string SpeakerName { get { return speakerName; } set { speakerName = value; } }

        [Header("Dialogue")]
        // [SerializeField] private string _speakerID;
        // public string SpeakerID { get { return _speakerID; } set { _speakerID = value; } }

        [SerializeField] private DialogueBlock[] _dialogue;
        public DialogueBlock[] Dialogue { get { return _dialogue; } set { _dialogue = value; } }

        [Header("Animation Handler")]
        [SerializeField] private NPCAnimationHandler _npcAnimationHandler;
        [SerializeField] public Transform eyeLevel;

        // Interactible Interface
        private bool _isInteractible = true;
        [HideInInspector] public bool isInteractible { get { return _isInteractible; } set { _isInteractible = value; } }
        [HideInInspector] public Transform ObjectTransform => transform;


        private Vector3 _ogRotation;

        [Header("Battle Stats")]
        [SerializeField] public BattleCharacterBase battleCharBaseStats;

        private string _currentMood = "";


#if UNITY_EDITOR
        private void OnValidate()
        {
            _npcAnimationHandler ??= TryGetComponent<NPCAnimationHandler>(out NPCAnimationHandler npcAnim) ? npcAnim : null;
        }
#endif


        private void OnEnable()
        {
            _npcAnimationHandler ??= TryGetComponent<NPCAnimationHandler>(out NPCAnimationHandler npcAnim) ? npcAnim : null;
        }

        private string GetDialogueString()
        {
            foreach (DialogueBlock dialogueBlock in _dialogue)
            {
                if (dialogueBlock.dialogueID != null)
                {
                    return dialogueBlock.dialogueID;
                }
            }

            return null;
        }

        public void Interact()
        {
            if (GetDialogueString() != null)
            {
                _ogRotation = transform.rotation.eulerAngles;
                RoateTowardsPlayer(true);
            }
            else
            {
                Debug.LogWarning("NPC doesn't have any dialogue");
            }
        }

        private void RoateTowardsPlayer(bool shouldTurn)
        {
            if (shouldTurn)
            {
                transform.DOLookAt(PlayerManager.instance.gameObject.transform.position, 1f);
            }
            else
            {
                transform.DOLookAt(_ogRotation, 1f);
            }
        }

        private void ChangeMood(string newMood)
        {
            if (newMood == _currentMood || newMood == "null")
                return;

            _currentMood = newMood;
            Debug.Log($"Mood Changed to '{newMood}'");
        }
    }
}
