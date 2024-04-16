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

    // MARK: Variables
    [HideInInspector] public PlayerLocomotion playerLocomotion;
    [HideInInspector] public PlayerAnimationHandler animationHandler;
    [HideInInspector] public Animator animator;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canRotate = true;
    [HideInInspector] public bool isPerformingActionFlag = false;
    [HideInInspector] public bool applyRootMotion; // animation
    [HideInInspector] public bool isInteracting = false; // interaction
    [HideInInspector] public FieldOfView fow;
    public IInteractibleBase currentInteractible;
    private float _delta;

    // MARK: PUBLIC:

    public void InteractibleNotNull() //Subscribe to inputManager, triggered by fow
    {
        GameManager.instance.gameEventManager.inputEvents.onInteractionInputPressed += InteractionKeyPressed;
    }

    public void UnSubscribeFromInteractible() // UnSubscribe form inputManager, triggered by fow
    {
        GameManager.instance.gameEventManager.inputEvents.onInteractionInputPressed -= InteractionKeyPressed;
    }

    public void InteractionKeyPressed() //triggered by inputManager in Freeroam, when there's an interactible
    {
        if (currentInteractible is ISpeakable speakableObject)
        {
            if (!isInteracting)
            {
                GameManager.instance.gameStateManager.SetState(GameState.Dialogue);
                GameManager.instance.dialogueManager.GetAppropriateDialogueString(speakableObject.Dialogue);
            }
        }

        currentInteractible.Interact();
        fow?.RemoveIcon();
    }

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

    // MARK: PRIVATE:
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
        GameManager.instance.gameStateManager.OnGameStateChanged += OnGameStateChanged;

        GameManager.instance.gameEventManager.playerEvents.onPlayerResumeControl += PlayerResumeControl;
        GameManager.instance.gameEventManager.playerEvents.onPlayerMoveToPosition += PlayerMoveToPosition;

        GameManager.instance.gameEventManager.checkpointEvents.onNewCheckPoint += OnCheckpointUpdate;
    }

    private void OnDisable()
    {
        GameManager.instance.gameStateManager.OnGameStateChanged -= OnGameStateChanged;

        GameManager.instance.gameEventManager.playerEvents.onPlayerResumeControl -= PlayerResumeControl;
        GameManager.instance.gameEventManager.playerEvents.onPlayerMoveToPosition -= PlayerMoveToPosition;

        GameManager.instance.gameEventManager.checkpointEvents.onNewCheckPoint -= OnCheckpointUpdate;
    }

    private void Start()
    {
        int myInt = GameManager.instance.checkpointManager.mostRecentCheckpointGUID;

        if (myInt != 0 && GameManager.instance.checkpointManager.allCheckpointsInScene != null)
        {
            foreach (Arcy.Scenes.Checkpoint checkpoint in GameManager.instance.checkpointManager.allCheckpointsInScene)
            {
                if (checkpoint.guid == myInt)
                {
                    if (checkpoint.spawnPoint != null)
                    {
                        transform.position = checkpoint.spawnPoint.position;
                        if (checkpoint.endPoint != null)
                        {
                            playerLocomotion.MoveToSpecificPosition(checkpoint.endPoint.position);
                        }
                    }

                    break;
                }
            }
        }
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

    private void PlayerResumeControl()
    {
        EnableMovement(true, false); // Re-enable Movement
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
        {
            playerLocomotion.HandleAllMovement(_delta);
        }
    }

    private void OnCheckpointUpdate(int checkPoint)
    {

    }
}
