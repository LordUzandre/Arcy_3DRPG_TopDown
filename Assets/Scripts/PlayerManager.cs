using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(CharacterController))]
public class PlayerManager : MonoBehaviour
{
    //Scripts
    [HideInInspector] public PlayerLocomotion playerLocomotion;
    [HideInInspector] public PlayerAnimationHandler animationHandler;
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public FieldOfView fow;
    [HideInInspector] public Interactible interactible;

    //Other assets
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    public static PlayerManager instance;

    private float delta;
    [Header("Flags")]
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canRotate = true;
    [HideInInspector] public bool isPerformingAction = false;
    [HideInInspector] public bool applyRootMotion;
    [HideInInspector] public bool isInteracting = false;
    [HideInInspector] public bool interactionFinished = false;

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

    private void Update()
    {
        delta = Time.deltaTime;

        if (isInteracting == false)
        {
            playerLocomotion.HandleAllMovement(delta);
            animationHandler.locomotion = InputManager.instance.moveAmount;
        }
    }

    public void interactionKeyPressed() //triggered by inputManager when there's an interactible
    {
        if (!isInteracting && !interactionFinished)
        {
            //if player is not already interacting with anything
            interactible.InteractStarted();
            isInteracting = true;

            playerLocomotion.enabled = false;
            fow.enabled = false;
        }
        else if (isInteracting && interactionFinished)
        {
            //when dialogue is finished
            playerLocomotion.enabled = true;
            fow.enabled = true;
            isInteracting = false;
            interactionFinished = false;
        }
    }

    public void ResetAfterDialogue()
    {
        playerLocomotion.enabled = true;
    }
}
