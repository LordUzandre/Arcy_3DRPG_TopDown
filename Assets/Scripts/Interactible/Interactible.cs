using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = System.Action;
using DG.Tweening;

public class Interactible : MonoBehaviour
{
    public bool isNPC;
    public bool haveDialogue;
    public float distanceToPlayer;
    public string speakerID;
    public Animator animator;
    public Dialogue dialogue;
    //public enum MyType { None, Chest, Sign, NPC, Door, Pickup, Enemy };

    //UI
    public static event System.Action<Vector3> MoveIconHere;
    public static event System.Action RemoveIcon;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            isNPC = false;
        }

        if (dialogue == null)
        {
            haveDialogue = false;
        }
    }

    //When the interactible is in focus, but not interacted with
    public void OnFocus()
    {
        if (MoveIconHere != null)
        {
            MoveIconHere(this.transform.position);
        }
    }

    //When the interactible is out of range for player
    public void OnDefocused()
    {
        if (RemoveIcon != null)
        {
            RemoveIcon();
        }
    }

    //Started by PlayerManager when the player is interacting this object
    public void InteractStarted()
    {
        if (haveDialogue)
        {
            DialogueManager.instance.MyMethod();
        }
    }
    public void TurnToPlayer(Vector3 playerPos)
    {
        transform.DOLookAt(playerPos, Vector3.Distance(transform.position, playerPos) / 5);
        string turnMotion = isRightSide(transform.forward, playerPos, Vector3.up) ? "rturn" : "lturn";
        animator.SetTrigger(turnMotion);
    }

    public bool isRightSide(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        // right vector
        Vector3 right = Vector3.Cross(up.normalized, fwd.normalized);
        float dir = Vector3.Dot(right, targetDir.normalized);
        return dir > 0f;
    }
}
