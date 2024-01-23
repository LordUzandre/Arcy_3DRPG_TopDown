using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Dialogue;
using Arcy.Animation;
using Arcy.InputManager;
using Arcy.Interaction;
using DG.Tweening;
using System;

[RequireComponent(typeof(CharacterController))]
public class PlayerManager : MonoBehaviour
{
    #region Player Scripts
    //Scripts
    [HideInInspector] public PlayerLocomotion playerLocomotion;
    [HideInInspector] public PlayerAnimationHandler animationHandler;
    [HideInInspector] public InputManager inputManager;
    //Other assets
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    #endregion

    //Singleton
    public static PlayerManager instance;

    #region movement variables
    private float delta;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canRotate = true;
    [HideInInspector] public bool isPerformingAction = false;
    [HideInInspector] public bool applyRootMotion;
    [HideInInspector] public bool isInteracting = false;
    #endregion

    #region interaction variables

    public static Action noObjectInFocus; //used by interactionIcon

    [HideInInspector] public FieldOfView fow;
    public InteractibleBase currentInteractible;
    #endregion

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

    #region GameState Subscription
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
        switch (state)
        {
            case GameState.Freeroam:
                EnableMovement(true, false);
                isInteracting = false;
                break;
            case GameState.Dialogue:
                EnableMovement(false, true);
                isInteracting = true;
                break;
            default:
                break;
        }
    }
    #endregion

    private void Update() //Should be the only Update() on player's scripts
    {
        delta = Time.deltaTime;

        if (canMove == true)
            playerLocomotion.HandleAllMovement(delta);
    }

    #region Interaction


    public void InteractibleNotNull() //Subscribe to inputManager, triggered by fow
    {
        InputManager.InteractionButtonPressed += InteractionKeyPressed;
    }

    public void UnSubscribeFromInteractible() // UnSubscribe form inputManager, triggered by fow
    {
        InputManager.InteractionButtonPressed -= InteractionKeyPressed;
    }

    public void InteractionKeyPressed() //triggered by inputManager in Freeroam, when there's an interactible
    {
        if (currentInteractible is ISpeakable speakableObject && speakableObject.SpeakerID != null)
        {
            if (!isInteracting)
                GameStateManager.Instance.SetState(GameState.Dialogue);

            DialogueManager.Instance.RunDialogue(speakableObject.SpeakerID);
        }
        else
        {
            currentInteractible.Interact();
        }

        fow?.RemoveIcon();
    }
    #endregion

    // Enable or disable character movement
    public void EnableMovement(bool canCharacterMove, bool rotateTowards = false)
    {
        if (rotateTowards)
        {
            // Rotate the player to face the modified target position
            Vector3 targetPosition = new Vector3(currentInteractible.ObjectTransform.position.x, transform.position.y, currentInteractible.ObjectTransform.position.z);
            transform.DOLookAt(targetPosition, 1f);
        }

        if (canCharacterMove == canMove)
            return;

        // Triggered when dialogue is finished
        isInteracting = !canCharacterMove;
        playerLocomotion.enabled = canCharacterMove;
        fow.enabled = canCharacterMove;
        canMove = canCharacterMove;
    }
}
