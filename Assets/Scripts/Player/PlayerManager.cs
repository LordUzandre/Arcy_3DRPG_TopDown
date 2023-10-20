using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Arcy.Dialogue;
using Arcy.Animation;
using Arcy.InputManager;

[RequireComponent(typeof(CharacterController))]
public class PlayerManager : MonoBehaviour
{
    //Scripts
    [HideInInspector] public PlayerLocomotion playerLocomotion;
    [HideInInspector] public PlayerAnimationHandler animationHandler;
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public FieldOfView fow;
    [HideInInspector] public Interactible currentInteractible;

    //Other assets
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    //Singleton
    public static PlayerManager instance;

    private float delta;
    [Header("Flags")]
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canRotate = true;
    [HideInInspector] public bool isPerformingAction = false;
    [HideInInspector] public bool applyRootMotion;
    [HideInInspector] public bool isInteracting = false;

    private void Awake()
    {
        //Movement scripts
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
        inputManager = GetComponent<InputManager>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        //Interaction
        fow = GetComponent<FieldOfView>();

        //Singleton
        if (instance == null) { instance = this; } else { Destroy(this); }
    }

    private void Update() //Should be the only Update() on player's scripts
    {
        delta = Time.deltaTime;

        if (isInteracting == false)
        {
            playerLocomotion.HandleAllMovement(delta);
        }

        animationHandler.locomotion = InputManager.instance.moveAmount;
    }

    public void interactionKeyPressed() //triggered by inputManager in Freeroam, when there's an interactible
    {
        if (!isInteracting && currentInteractible.hasDialogue)
        {
            DialogueManager.instance.RunDialogue(currentInteractible);
            isInteracting = true;
            playerLocomotion.enabled = false;
            fow.enabled = false;
        }
        else if (isInteracting && currentInteractible.hasDialogue)
        {
            DialogueManager.instance.RunDialogue(currentInteractible);
        }
    }

    public void ResetAfterDialogue()
    {
        //when dialogue is finished
        isInteracting = false;
        playerLocomotion.enabled = true;
        fow.enabled = true;
    }
}
