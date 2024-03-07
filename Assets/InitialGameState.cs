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

        private void Awake()
        {
            GameEventManager.instance.gameStateManager.SetState(GameState.RedundantGameState);
        }

        private void Start()
        {
            StartCoroutine(InitialDelay());

            IEnumerator InitialDelay()
            {
                yield return new WaitForSeconds(.5f);
                GameEventManager.instance.gameStateManager.SetState(_startingGameState);
            }
        }

    }
}
