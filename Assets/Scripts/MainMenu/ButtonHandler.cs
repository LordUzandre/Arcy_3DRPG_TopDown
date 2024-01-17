using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Arcy.MainMenu
{
    public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        /// <summary>
        /// This class is used to navigate the buttons in the main menu
        /// </summary>

        //Attach this script to buttons
        [SerializeField] private float _horizontalMoveAmount;
        [SerializeField] private float _verticalMoveAmount;
        [SerializeField] private float _moveTime = .1f;
        [Range(0f, 2f), SerializeField] private float _scaleAmount;

        private Vector3 _startPos;
        private Vector2 _startScale;

        private void Start()
        {
            _startPos = this.transform.position;
            _startScale = transform.localScale;
        }

        private IEnumerator MoveSelection(bool startAnimation)
        {
            Vector3 endPosition;
            Vector3 endScale;
            float elapsedTime = Time.time + _moveTime;

            while (Time.time < elapsedTime)
            {
                if (startAnimation)
                {
                    endPosition = _startPos + new Vector3(_horizontalMoveAmount, _verticalMoveAmount, 0f);
                    endScale = _startScale * _scaleAmount;
                }
                else
                {
                    endPosition = _startPos;
                    endScale = _startScale;
                }
            }
            yield return null;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            //When mouse clicker enters a new button
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //when mouse pointer exits a button
        }

        public void OnSelect(BaseEventData eventData)
        {
            //when the button is selected by buttons
        }

        public void OnDeselect(BaseEventData eventData)
        {
            //when the selector leaves
        }
    }
}
