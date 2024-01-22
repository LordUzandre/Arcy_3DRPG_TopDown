using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Animation;
using Arcy.Dialogue;

namespace Arcy.Interaction
{
    public class NPCBase : MonoBehaviour, InteractibleBase, ISpeakable
    {
        [Header("Dialogue")]
        [SerializeField] private string _speakerID;
        public string SpeakerID { get { return _speakerID; } set { _speakerID = value; } }

        [Header("Animation Handler")]
        [SerializeField] private NPCAnimationHandler _npcAnimationHandler;
        [SerializeField] public Transform eyeLevel;

        // Interactible Interface
        private bool _isInteractible = true;
        [HideInInspector] public bool isInteractible { get { return _isInteractible; } set { _isInteractible = value; } }
        [HideInInspector] public Transform ObjectTransform => transform;


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

        public void Interact()
        {
            if (_speakerID != null)
                _npcAnimationHandler.anim.SetTrigger("talking");
            else
                Debug.Log("NPC doesn't have any dialogue");
        }
    }
}
