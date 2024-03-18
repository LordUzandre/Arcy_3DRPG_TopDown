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
            // Spawn the loot
            if (loot != null)
            {
                StartCoroutine(SpawnRoutine());
                IEnumerator SpawnRoutine()
                {
                    yield return null;
                    // Get the location of the player
                    Vector3 playerLocation = GameObject.FindGameObjectWithTag("Player").transform.position;

                    foreach (Inventory.InventorySlot item in loot)
                    {
                        yield return null;
                        if (item.prefab != null)
                        {
                            for (int i = 0; i < item.amount; i++)
                            {
                                yield return null;
                                // TODO - Create a random rotation for the prefab
                                GameObject loot = Instantiate<GameObject>(item.prefab);

                                // Shoot the loot in the player's general location
                                Rigidbody rb = loot.TryGetComponent<Rigidbody>(out Rigidbody hit) ? hit : null;
                                rb.velocity = playerLocation + new Vector3(0, 10, 0);
                                yield return null;
                            }
                        }
                    }
                }
            }

        }

    }
}
