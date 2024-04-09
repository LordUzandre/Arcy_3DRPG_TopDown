using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Dialogue;

namespace Arcy.Interaction
{
    [RequireComponent(typeof(BoxCollider))]
    public class Chest : MonoBehaviour, InteractibleBase
    {
        [SerializeField] BoxCollider _boxCollider;
        [SerializeField] Animator _anim;
        [SerializeField] Inventory.InventorySlot[] loot;

        [HideInInspector] public Transform ObjectTransform => transform;

        private bool _isInteractible = true;
        [HideInInspector] public bool isInteractible { get { return _isInteractible; } set { _isInteractible = value; } }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _boxCollider ??= TryGetComponent<BoxCollider>(out BoxCollider boxCollider) ? _boxCollider = boxCollider : null;
            _anim ??= GetComponentInChildren<Animator>();
        }
#endif

        public void Interact()
        {
            _anim.SetTrigger("Opening");
            // TODO - Spawn the loot
        }

    }
}
