using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Arcy.Camera;

namespace Arcy.Interaction
{
    public class InteractionIcon : MonoBehaviour
    {
        [SerializeField] private Transform _mainCamera;
        [SerializeField] private Image _interactionIcon;
        [SerializeField] private CanvasGroup _cvGroup;
        private Vector3 _punchScale = new Vector3(1f, 1f, 1f);
        private Vector3 _originalScale;
        private Vector3 _canvasOffset;
        private float _yOffset = 2f;
        private float _zOffset = -0.5f;

#if UINTY_EDITOR
        private void OnValidate()
        {
            CheckComponents();
        }
#endif

        private void Start()
        {
            CheckComponents();

            _canvasOffset = new Vector3(0, _yOffset, _zOffset);
            _cvGroup.alpha = 0;
        }

        private void CheckComponents()
        {
            //Replace with future camera system (?)
            _mainCamera ??= UnityEngine.Camera.main.transform;
            _interactionIcon ??= GetComponentInChildren<Image>();
            _cvGroup ??= TryGetComponent<CanvasGroup>(out CanvasGroup cv) ? cv : null;
            _originalScale = this.transform.localScale;
        }

        private void OnEnable()
        {
            //subscribe to events
            FieldOfView.moveInteractionIconHere += NewInteractibleInFocus;
            FieldOfView.noObjectInFocus += NoObjectInFocus;

        }

        private void OnDisable()
        {
            //de-subscribe from events
            FieldOfView.moveInteractionIconHere -= NewInteractibleInFocus;
            FieldOfView.noObjectInFocus -= NoObjectInFocus;
        }

        private void Update()
        {
            Quaternion transformRotation = _mainCamera.transform.rotation;
            transform.rotation = Quaternion.Euler(transformRotation.eulerAngles.x, 0f, 0f);
        }

        private void NewInteractibleInFocus(Vector3 newPosition)
        {
            this.transform.localScale = _originalScale;
            this.transform.position = newPosition + _canvasOffset;
            _cvGroup.alpha = 1;
            transform.DOPunchScale(_punchScale, .2f, 6, .5f);
        }

        private void NoObjectInFocus()
        {
            _cvGroup.alpha = 0;
        }
    }

}