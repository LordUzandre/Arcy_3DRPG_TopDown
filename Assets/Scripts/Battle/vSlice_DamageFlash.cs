using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Battle
{
    public class vSlice_DamageFlash : MonoBehaviour
    {
        private Renderer[] _rs;

        private void Start()
        {
            _rs = GetComponentsInChildren<Renderer>();
        }

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

        private void SetMaterialEmission(Color color)
        {
            for (int i = 0; i < _rs.Length; i++)
            {
                _rs[i].material.SetColor("_EmissionColor", color);
            }
        }
    }
}
