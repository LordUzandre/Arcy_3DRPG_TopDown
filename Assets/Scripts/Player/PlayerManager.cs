using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Dialogue;
using Arcy.Animation;
using Arcy.InputManagement;
using Arcy.Interaction;
using Arcy.Player;
using DG.Tweening;
using System;
using Arcy.Management;

[RequireComponent(typeof(CharacterController))]
public class PlayerManager : MonoBehaviour
{
    //Singleton
    public static PlayerManager instance;

    #region Variables
    //Scripts
    [HideInInspector] public PlayerLocomotion playerLocomotion;
    [HideInInspector] public PlayerAnimationHandler animationHandler;

    //Other assets
    // [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    private float _delta;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canRotate = true;
    [HideInInspector] public bool isPerformingActionFlag = false;
    [HideInInspector] public bool applyRootMotion; // animation
    [HideInInspector] public bool isInteracting = false; // interaction

    [HideInInspector] public FieldOfView fow;
    public InteractibleBase currentInteractible;
    #endregion

    private void Awake()
    {
        //Movement scripts
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
        animator = GetComponentInChildren<Animator>();

        //Interaction
        fow = GetComponent<FieldOfView>();

        //Singleton
        if (instance == null) { instance = this; } else { Destroy(this); }
    }

    private void OnEnable()
    {
        GameEventManager.instance.gameStateManager.OnGameStateChanged += OnGameStateChanged;
        GameEventManager.instance.playerEvents.onPlayerResumeControl += PlayerReumeControl;
        GameEventManager.instance.playerEvents.onPlayerLevelUp += PlayerLevelUp;
        GameEventManager.instance.playerEvents.onPlayerMoveToPosition += PlayerMoveToPosition;
    }

    private void OnDisable()
    {
        GameEventManager.instance.gameStateManager.OnGameStateChanged -= OnGameStateChanged;
        GameEventManager.instance.playerEvents.onPlayerResumeControl -= PlayerReumeControl;
        GameEventManager.instance.playerEvents.onPlayerLevelUp -= PlayerLevelUp;
        GameEventManager.instance.playerEvents.onPlayerMoveToPosition -= PlayerMoveToPosition;
    }

    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Freeroam:
                EnableMovement(true, false); // Enable Movement
                isInteracting = false;
                return;
            case GameState.Dialogue:
                EnableMovement(false, true); // Disable Movement and rotate towrds the speaker
                isInteracting = true;
                return;
            default:
                EnableMovement(false); // Disable Movement and hold still
                return;
        }
    }

    private void PlayerReumeControl()
    {
        EnableMovement(true, false); // Re-enable Movement
    }

    private void PlayerLevelUp()
    {

    }

    private void PlayerMoveToPosition(Vector3 newPos)
    {
        EnableMovement(false, true); // Disable player control and rotate towards the new target
        playerLocomotion.MoveToSpecificPosition(newPos);
    }

    private void Update() //Should be the only Update() on player's scripts
    {
        _delta = Time.deltaTime;

        if (canMove == true)
            playerLocomotion.HandleAllMovement(_delta);
    }

    #region Interaction

    public void InteractibleNotNull() //Subscribe to inputManager, triggered by fow
    {
        GameEventManager.instance.inputEvents.onInteractionInputPressed += InteractionKeyPressed;
    }

    public void UnSubscribeFromInteractible() // UnSubscribe form inputManager, triggered by fow
    {
        GameEventManager.instance.inputEvents.onInteractionInputPressed -= InteractionKeyPressed;
    }

    public void InteractionKeyPressed() //triggered by inputManager in Freeroam, when there's an interactible
    {
        if (currentInteractible is ISpeakable speakableObject) // && speakableObject.SpeakerID != null)
        {
            if (!isInteracting)
            {
                GameEventManager.instance.gameStateManager.SetState(GameState.Dialogue);
                DialogueManager.Instance.GetAppropriateDialogueString(speakableObject.Dialogue);
            }

            // DialogueManager.Instance.RunDialogue(speakableObject.SpeakerID);
        }

        currentInteractible.Interact();
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
