using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    public class vSlice_DamageFlash : MonoBehaviour
    {
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
                WaitForSeconds delay = new WaitForSeconds(0.2f);

                for (int i = 0; i < 3; i++)
                {
                    SetMaterialEmission(Color.white);
                    yield return delay;
                    SetMaterialEmission(Color.white);
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
