using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Saving;

namespace Arcy.Interaction
{
    [RequireComponent(typeof(BoxCollider))]
    public class Chest : MonoBehaviour, IInteractibleBase
    {
        /// <summary>
        /// If this object is a child of single-time spawner - then it will take care the save-data
        /// </summary>
        /// 
        [HideInInspector] public Transform ObjectTransform => transform; // Used by InteractibleIcon
        [SerializeField] BoxCollider _boxCollider;
        [SerializeField] Animator _anim;
        [Header("Loot")]
        [SerializeField] Inventory.InventorySlot[] loot;

        [Header("For Save-data")]
        [SerializeField] public int guid = 0;
        [SerializeField] private bool _isInteractible = true;
        public bool isInteractible { get { return _isInteractible; } set { _isInteractible = value; } }

        // MARK: PUBLIC:

        public void Interact()
        {
            if (_isInteractible)
            {
                _anim.SetTrigger("Opening");
                _isInteractible = false;

                foreach (Inventory.InventorySlot slot in loot)
                {
                    if (slot.GetItem() != null && slot.GetAmount() > 0)
                    {
                        // TODO - Spawn the loot & Add To Inventory
                    }
                }
            }
        }

        public void SetStartState(bool isOpen)
        {
            _anim.SetBool("isOpen", isOpen);
        }

        // MARK: PRIVATE

#if UNITY_EDITOR
        private void OnValidate()
        {
            _boxCollider ??= TryGetComponent<BoxCollider>(out BoxCollider boxCollider) ? _boxCollider = boxCollider : null;
            _anim ??= GetComponentInChildren<Animator>();
            if (guid == 0) guid = Utils.GuidGenerator.guid();
        }
#endif
    }
}
