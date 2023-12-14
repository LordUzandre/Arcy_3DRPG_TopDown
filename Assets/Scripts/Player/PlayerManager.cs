using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Dialogue;
using Arcy.Animation;
using Arcy.InputManager;
using Arcy.Interaction;
using System;

[RequireComponent(typeof(CharacterController))]
public class PlayerManager : MonoBehaviour
{
    //Scripts
    [HideInInspector] public PlayerLocomotion playerLocomotion;
    [HideInInspector] public PlayerAnimationHandler animationHandler;
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public FieldOfView fow;
    [HideInInspector] public InteractibleBase currentInteractible;

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

    private void OnEnable()
    {
        GameStateManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameStateManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {

    }

    private void Update() //Should be the only Update() on player's scripts
    {
        delta = Time.deltaTime;

        if (canMove == true)
        {
            playerLocomotion.HandleAllMovement(delta);
            animationHandler.locomotion = InputManager.instance.moveAmount;
        }
    }

    public void interactionKeyPressed() //triggered by inputManager in Freeroam, when there's an interactible
    {
        if (currentInteractible is ISpeakable speakableObject && speakableObject.SpeakerID != null)
        {
            if (!isInteracting)
            {
                EnableMovement(false);
            }

            bool cameraShouldChange = currentInteractible is NPCBase ? true : false; // Should be in CameraManager
            DialogueManager.instance.RunDialogue(currentInteractible, speakableObject.SpeakerID, currentInteractible.transform, cameraShouldChange);
        }
        else
        {
            currentInteractible.Interact();
        }
    }

    public void EnableMovement(bool canCharacterMove)
    {
        //when dialogue is finished
        isInteracting = !canCharacterMove;

        playerLocomotion.enabled = canCharacterMove;
        fow.enabled = canCharacterMove;
        canMove = canCharacterMove;
    }
}
