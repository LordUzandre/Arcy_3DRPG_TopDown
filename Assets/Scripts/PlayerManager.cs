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

    private float delta;
    [Header("Flags")]
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canRotate = true;
    [HideInInspector] public bool isPerformingAction = false;
    [HideInInspector] public bool applyRootMotion;

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
    }

    private void Update()
    {
        delta = Time.deltaTime;

        playerLocomotion.HandleAllMovement(delta);
        animationHandler.locomotion = InputManager.instance.moveAmount;
    }
}
