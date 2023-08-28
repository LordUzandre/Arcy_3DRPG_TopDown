using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private PlayerInputs playerInputs;

    [Header("Player Movement")]
    [SerializeField] Vector2 movementInput;
    public float inputY;
    public float inputX;
    public float moveAmount;

    [Header("Action Inputs")]
    public bool dodgeInput = false;

    private void Awake()
    {
        if (instance == null) { instance = this; } else { Destroy(this); }
    }

    private void OnEnable()
    {
        if (playerInputs == null)
        {
            //movementInput
            playerInputs = new PlayerInputs();
            playerInputs.Gameplay.move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerInputs.Gameplay.move.canceled += i => movementInput = i.ReadValue<Vector2>();

            //dodge- and runInput
            playerInputs.Gameplay.run.performed += i => dodgeInput = true;

            //interactInput
            playerInputs.Gameplay.interact.performed += CheckInteractible;
        }

        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    // When player presses "E"-key
    private void CheckInteractible(InputAction.CallbackContext context)
    {
        if (PlayerManager.instance.interactible != null)
        {
            PlayerManager.instance.interactionKeyPressed();
        }
    }

    private void Update()
    {
        //Which gaestate are we currently in?
        switch (GameStateManager.Instance.CurrentGameState)
        {
            case GameState.Freeroam:
                HandleMovementInput();
                break;
            default:
                break;
        }
    }

    private void HandleMovementInput()
    {
        inputY = movementInput.y;
        inputX = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(inputY) + Mathf.Abs(inputX));

        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }
    }

    //  Actions
    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;

            //  FUTURE NOTE: RETURN (DO NOTHING) IF MENU OR UI WINDOW IS OPEN
            //player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }
}