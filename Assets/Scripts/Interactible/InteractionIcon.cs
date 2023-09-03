using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractionIcon : MonoBehaviour
{
    Transform mainCamera;
    UnityEngine.UI.Image interactionIcon;
    Vector3 originalScale;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<Transform>();
        interactionIcon = GetComponentInChildren<UnityEngine.UI.Image>();
        interactionIcon.enabled = false;
        originalScale = this.transform.localScale;
    }

    private void OnEnable()
    {
        Interactible.MoveIconHere += SwitchObject;
        Interactible.RemoveIcon += NoObjectInFocus;
    }

    private void OnDisable()
    {
        Interactible.MoveIconHere -= SwitchObject;
        Interactible.RemoveIcon -= NoObjectInFocus;
    }

    private void Update()
    {
        Quaternion transformRotation = mainCamera.transform.rotation;
        transform.rotation = Quaternion.Euler(transformRotation.eulerAngles.x, 0f, 0f);
    }

    private void SwitchObject(Vector3 newPosition)
    {
        this.transform.localScale = originalScale;
        this.transform.position = new Vector3(0f, 1f, 0f) + newPosition;
        interactionIcon.enabled = true;
        transform.DOPunchScale(new Vector3(1f, 1f, 1f), .3f, 5, .5f);
    }

    private void NoObjectInFocus()
    {
        interactionIcon.enabled = false;
    }
}
