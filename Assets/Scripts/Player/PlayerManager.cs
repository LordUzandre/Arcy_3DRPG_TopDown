using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Animation;
using Arcy.Interaction;
using Arcy.Player;
using DG.Tweening;
using Arcy.Management;

namespace Arcy.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerManager : MonoBehaviour
    {
        //Singleton
        public static PlayerManager instance;
        public static Collider player;

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
            if (currentInteractible is Dialogue.ISpeakable speakableObject)
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
            player = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            GameManager.instance.gameStateManager.OnGameStateChanged += GameStateChanged;

            GameManager.instance.gameEventManager.playerEvents.onPlayerResumeControl += PlayerResumeControl;
            GameManager.instance.gameEventManager.playerEvents.onPlayerMoveToPosition += PlayerMoveToPosition;
        }

        private void OnDisable()
        {
            GameManager.instance.gameStateManager.OnGameStateChanged -= GameStateChanged;

            GameManager.instance.gameEventManager.playerEvents.onPlayerResumeControl -= PlayerResumeControl;
            GameManager.instance.gameEventManager.playerEvents.onPlayerMoveToPosition -= PlayerMoveToPosition;
        }

        private void Start()
        {
            StartAtCheckpoint();
        }

        private void Update() //Should be the only Update() on player's scripts
        {
            if (canMove == true)
            {
                playerLocomotion.HandleAllMovement();
            }
        }

        // Should be the only part f the player subscibing to these events.
        private void GameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Freeroam:
                    EnableMovement(true, false); // Enable Movement
                    isInteracting = false;
                    playerLocomotion.characterIsMovingSomewhere = false;
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
            playerLocomotion.MoveToSpecifiedPosition(newPos);
        }

        private void StartAtCheckpoint()
        {
            int mostRecentCheckpointGuid = GameManager.instance.checkpointManager.mostRecentCheckpointGUID;

            if (mostRecentCheckpointGuid != 0 && GameManager.instance.checkpointManager.allCheckpointsInScene != null)
            {
                foreach (Arcy.Scenes.Checkpoint checkpoint in GameManager.instance.checkpointManager.allCheckpointsInScene)
                {
                    if (checkpoint.guid == mostRecentCheckpointGuid)
                    {
                        if (checkpoint.spawnPoint != null)
                        {
                            transform.position = checkpoint.spawnPoint.position;
                            if (checkpoint.endPoint != null && playerLocomotion.startAtCheckpoint)
                            {
                                playerLocomotion.MoveToSpecifiedPosition(checkpoint.endPoint.position);
                            }
                        }

                        break;
                    }
                }
            }
        }
    }

}