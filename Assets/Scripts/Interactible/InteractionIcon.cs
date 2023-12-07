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

        private void CheckComponents()
        {
            //Replace with future camera system
            if (mainCamera == null)
            {
                mainCamera = CameraManager.instance.mainCamera.transform;

                if (mainCamera == null)
                {
                    mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
                    print("Interaction icon couldn't find camera");
                }
            }

            if (interactionIcon == null)
            {
                interactionIcon = GetComponentInChildren<Image>();
            }

            cvGroup = GetComponent<CanvasGroup>();
            cvGroup.alpha = 0;
            originalScale = this.transform.localScale;
        }

        private void OnEnable()
        {
            //subscribe to events
            FieldOfView.moveInteractionIconHere += NewInteractibleInFocus;

        }

        private void OnDisable()
        {
            //de-subscribe from events
            FieldOfView.moveInteractionIconHere -= NewInteractibleInFocus;
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