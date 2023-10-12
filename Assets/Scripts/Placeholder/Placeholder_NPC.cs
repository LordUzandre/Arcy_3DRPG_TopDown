using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeholder_NPC : MonoBehaviour
{
    public Animator animator;
    [HideInInspector] public bool canMove, canRotate, isPerformingAction;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

}
