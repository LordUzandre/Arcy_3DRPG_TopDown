using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Arcy.Dialogue;
using Arcy.Animation;
using Arcy.InputManager;
using Arcy.Interaction;

[RequireComponent(typeof(CharacterController))]
public class PlayerManager : MonoBehaviour
{
    //Singleton
    public static PlayerManager instance;

    //Scripts
    [HideInInspector] public PlayerLocomotion playerLocomotion;
    [HideInInspector] public PlayerAnimationHandler animationHandler;
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public FieldOfView fow;
    [HideInInspector] public IInteractible currentInteractible;

    //Other assets
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canRotate = true;
    [HideInInspector] public bool isPerformingAction = false;
    [HideInInspector] public bool applyRootMotion;
    [HideInInspector] public bool isInteracting = false;
    private float delta;

    private void Reset()
    {
        CheckComponents();
    }

    private void Awake()
    {
        CheckComponents();

        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void CheckComponents()
    {
        if (playerLocomotion == null)
        {
            playerLocomotion = GetComponent<PlayerLocomotion>();
        }

        if (animationHandler == null)
        {
            animationHandler = GetComponent<PlayerAnimationHandler>();
        }

        if (inputManager == null)
        {
            inputManager = GetComponent<InputManager>();
        }

        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (fow == null)
        {
            fow = GetComponent<FieldOfView>();
        }
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
        currentInteractible.Interact();

        //The code below needs to be replaced to fit interface system

        // if (!isInteracting && currentInteractible is ISpeakable)
        // {
        //     DialogueManager.instance.RunDialogue(currentInteractible);
        //     isInteracting = true;
        //     playerLocomotion.enabled = false;
        //     fow.enabled = false;
        // }
        // else if (isInteracting && currentInteractible is ISpeakable)
        // {
        //     DialogueManager.instance.RunDialogue(currentInteractible);
        // }
    }

    public void ResetAfterDialogue()
    {
        //when dialogue is finished
        isInteracting = false;
        playerLocomotion.enabled = true;
        fow.enabled = true;
    }
}
