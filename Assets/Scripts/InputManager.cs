using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InputManager();
            }
            return instance;
        }
    }

    private PlayerInputs playerInputs;

    [Header("Player Movement")]
    [SerializeField] Vector2 movementInput;
    public float inputY;
    public float inputX;
    public float moveAmount;

    [Header("Action Inputs")]
    public bool dodgeInput = false;

    private void Start()
    {
        instance.enabled = true;
    }

    private void OnEnable()
    {
        if (playerInputs == null)
        {
            playerInputs = new PlayerInputs();
            playerInputs.Gameplay.move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerInputs.Gameplay.move.canceled += i => movementInput = i.ReadValue<Vector2>();
            playerInputs.Gameplay.run.performed += i => dodgeInput = true;
            playerInputs.Gameplay.interact.performed += CheckInteractible;
        }

        playerInputs.Enable();
    }

    // When player presses "E"-key
    private void CheckInteractible(InputAction.CallbackContext context)
    {
        if (PlayerManager.Instance.interactible != null)
        {
            PlayerManager.Instance.InteractionStarted();
        }
    }

    private void Update()
    {
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