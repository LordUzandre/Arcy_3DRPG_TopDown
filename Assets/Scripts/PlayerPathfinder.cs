using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Pathfinding
{
    public class PlayerPathfinder : MonoBehaviour
    {
        private Vector3 _ogPosition = new Vector3(0, 0, 0);

        [SerializeField] private GameObject _indicationObject;
        private PlayerManager _playerManager;
        private CharacterController _characterController;

        public static Action moveToNewPosition;

        private void OnEnable()
        {
            _playerManager = PlayerManager.instance;

            //subscribe to moveToNewPosition
        }

        private void OnDisable()
        {
            //De-subscribe to delegates
        }

        private void FooMethod(Vector3 _newPosition)
        {
            StartCoroutine(newPosition(_newPosition));
        }

        private IEnumerator newPosition(Vector3 destinationPosition)
        {
            Vector3 desiredRotation = new Vector3(0, 0, 0);

            //Set a new position and use characterController to move towards it.
            while (true)
            {
                yield return null;
                _characterController.Move(desiredRotation);
            }
        }
    }
}
