using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Arcy.Interaction
{
    public class InteractionIcon : MonoBehaviour
    {
        [SerializeField] Transform mainCamera;
        [SerializeField] Image interactionIcon;
        [SerializeField] CanvasGroup cvGroup;
        private Vector3 originalScale;

        private void Reset()
        {
            CheckComponents();
        }

        private void Start()
        {
            CheckComponents();
        }

        private void OnEnable()
        {
            //subscribe to events
            Interactible.MoveIconHere += NewInteractibleInFocus;
            Interactible.RemoveIcon += NoObjectInFocus;
        }

        private void CheckComponents()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main.transform;
            }

            if (interactionIcon == null)
            {
            interactionIcon = GetComponentInChildren<Image>();
            }
            
            cvGroup = GetComponent<CanvasGroup>();
            cvGroup.alpha = 0;
            originalScale = this.transform.localScale;
        }

        private void OnDisable()
        {
            //de-subscribe from events
            Interactible.MoveIconHere -= NewInteractibleInFocus;
            Interactible.RemoveIcon -= NoObjectInFocus;
        }

        private void Update()
        {
            Quaternion transformRotation = mainCamera.transform.rotation;
            transform.rotation = Quaternion.Euler(transformRotation.eulerAngles.x, 0f, 0f);
        }

        private void NewInteractibleInFocus(Vector3 newPosition)
        {
            this.transform.localScale = originalScale;
            this.transform.position = new Vector3(0f, 1f, 0f) + newPosition;
            cvGroup.alpha = 1;
            transform.DOPunchScale(new Vector3(1f, 1f, 1f), .3f, 5, .5f);
        }

        private void NoObjectInFocus()
        {
            cvGroup.alpha = 0;
        }
    }

}