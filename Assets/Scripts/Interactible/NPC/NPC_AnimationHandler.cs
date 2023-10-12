using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPC_AnimationHandler : MonoBehaviour
{
    public Placeholder_NPC npc;

    private void Start()
    {
        if (npc == null)
        {
            npc = GetComponent<Placeholder_NPC>();
        }
    }

    IEnumerator AnimateNPC()
    {
        while (true)
        {
            float animatorFloat = 0; // = value for locomotion, SET UP LATER!
            float dampTime = 1f; // = how long should it take to blend?
            npc.animator.SetFloat("Blend", animatorFloat, dampTime, Time.deltaTime);
            yield return null;
        }
    }

    public void TurnToPlayer(Vector3 playerPos) // triggered by dialogueManager
    {
        //transform.DOLookAt(playerPos, Vector3.Distance(transform.position, playerPos) / 5);
        string turnMotion = isRightSide(transform.forward, playerPos, Vector3.up) ? "rTurn" : "lTurn";
        npc.animator.SetTrigger(turnMotion);
    }

    public bool isRightSide(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        // right vector
        Vector3 right = Vector3.Cross(up.normalized, fwd.normalized);
        float dir = Vector3.Dot(right, targetDir.normalized);
        return dir > 0f;
    }

    public void PlayNewAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
    {
        npc.animator.applyRootMotion = applyRootMotion;
        npc.animator.CrossFade(targetAnimation, 0.2f);
        //  CAN BE USED TO STOP CHARACTER FROM ATTEMPTING NEW ACTIONS
        //  FOR EXAMPLE, IF YOU GET DAMAGED, AND BEGIN PERFORMING A DAMAGE ANIMATION
        //  THIS FLAG WILL TURN TRUE IF YOU ARE STUNNED
        //  WE CAN THEN CHECK FOR THIS BEFORE ATTEMPTING NEW ACTIONS
        npc.isPerformingAction = isPerformingAction;
        npc.canRotate = canRotate;
        npc.canMove = canMove;
    }
}
