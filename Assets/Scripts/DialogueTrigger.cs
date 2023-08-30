using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager ui;
    private Interactible currentInteractible;
    private PlayerLocomotion playerLocomotion;
    public CinemachineTargetGroup targetGroup;

    [Space]

    [Header("Post Processing")]
    public Volume dialogueDof;

    void Start()
    {
        ui = DialogueManager.instance;
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !ui.currentlyInDialogue && currentInteractible != null)
        {
            //camera settings
            targetGroup.m_Targets[1].target = currentInteractible.transform;
            //disable locomotion
            playerLocomotion.enabled = false;
            // UI
            //ui.SetNameTextAndColor();
            ui.currentlyInDialogue = true;
            ui.CameraChange(true);
            ui.ClearText();
            ui.ShowUI(true, .2f, .65f);
            currentInteractible.TurnToPlayer(transform.position);
            currentInteractible = PlayerManager.instance.interactible;
        }
    }
}
