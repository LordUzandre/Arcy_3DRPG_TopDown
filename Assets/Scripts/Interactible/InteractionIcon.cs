using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Arcy.Management;

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
        private void Reset()
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
            //Replace with future camera system
            if (_mainCamera == null)
            {
                _mainCamera = UnityEngine.Camera.main.transform;
                print("Interaction icon couldn't find camera");
            }

            if (_interactionIcon == null)
                _interactionIcon = GetComponentInChildren<Image>();

            _cvGroup = TryGetComponent<CanvasGroup>(out CanvasGroup cv) ? _cvGroup = cv : null;
            _originalScale = this.transform.localScale;
        }

        private void OnEnable()
        {
            //subscribe to events
            GameManager.instance.gameEventManager.interactionEvents.onMoveInteractionIconHere += NewInteractibleInFocus;
            GameManager.instance.gameEventManager.interactionEvents.onNoObjectInFocus += NoObjectInFocus;

        }

        private void OnDisable()
        {
            //de-subscribe from events
            GameManager.instance.gameEventManager.interactionEvents.onMoveInteractionIconHere -= NewInteractibleInFocus;
            GameManager.instance.gameEventManager.interactionEvents.onNoObjectInFocus -= NoObjectInFocus;
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
            transform.DOPunchScale(_punchScale, .2f, 5, .5f);
        }

        private void NoObjectInFocus()
        {
            _cvGroup.DOFade(0, 0.25f);
        }
    }

}