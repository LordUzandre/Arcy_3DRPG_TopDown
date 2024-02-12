using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Management
{
    public class InitialGameState : MonoBehaviour
    {
        /// <summary>
        /// This class is used only for debug purposes.
        /// </summary>

        [SerializeField] private GameState _startingGameState;

        private void Start()
        {
            StartCoroutine(InitialDelay());
        }

        IEnumerator InitialDelay()
        {
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            GameStateManager.Instance.SetState(_startingGameState);
        }
    }
}
