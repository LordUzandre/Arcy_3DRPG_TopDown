using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [HideInInspector] public CinemachineVirtualCamera mainCamera;
    private Transform player;
    public float distanceToPlayer;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        distanceToPlayer = Vector3.Distance(this.transform.position, player.position);
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, distanceToPlayer))
        {
        }
    }
}