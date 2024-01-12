using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    public class DamageFlash : MonoBehaviour
    {
        /// <summary>
        /// This class is used to visually indicate that that a character has taken damage. 
        /// Remember that material can't use baked lights
        /// </summary>

        private Renderer[] _rs; // All childObjects with attached materials

        private void Start()
        {
            _rs = GetComponentsInChildren<Renderer>();
        }

        // Called by CharacterBase whenever character takes damage
        public void Flash()
        {
            StartCoroutine(FlashCoroutine());

            IEnumerator FlashCoroutine()
            {
                WaitForSeconds delay = new WaitForSeconds(0.05f);

                for (int i = 0; i < 2; i++)
                {
                    SetMaterialEmission(Color.white);
                    yield return delay;
                    SetMaterialEmission(Color.black);
                    yield return delay;
                }
            }
        }

        // Sets the renderer's material emission color.
        private void SetMaterialEmission(Color color)
        {
            for (int i = 0; i < _rs.Length; i++)
            {
                _rs[i].material.SetColor("_EmissionColor", color);
            }
        }
    }
}
