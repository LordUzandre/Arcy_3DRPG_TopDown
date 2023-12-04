using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Main Camera")]
    public CinemachineVirtualCamera mainCamera;
    [Space]
    [Header("Dialogue Camera")]
    public CinemachineVirtualCamera dialogueCamera;
    [Space]
    [Header("Top View Camera")]
    public CinemachineVirtualCamera topViewCamera;
    [HideInInspector] public static CameraManager instance;
    [HideInInspector] public float distanceToPlayer;
    public static Action changeCamera;

    //Private:
    private Transform player;

    private void Reset()
    {
        CheckComponents();
    }

    private void Start()
    {
        // if (instance == null)
        // {
        //     instance = this;
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }

    private void OnEnable()
    {
        CheckComponents();
        changeCamera += ChangeCameraView;
    }

    private void OnDisable()
    {
        changeCamera -= ChangeCameraView;
    }

    private void CheckComponents()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
        }

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        distanceToPlayer = Vector3.Distance(this.transform.position, player.position);
    }

    void FixedUpdate()
    {
        //Change to top view camera if blocked
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, distanceToPlayer))
        {
        }
    }

    public void ChangeCameraView()
    {

    }
}