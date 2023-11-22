using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public bool isNPC;
    public bool hasDialogue;
    public float distanceToPlayer;
    public string speakerID;
    public Animator animator;
    public DialogueBlock dialogue;
    //public enum MyType { None, Chest, Sign, NPC, Door, Pickup, Enemy };

    //UI
    public static Action<Vector3> MoveIconHere;
    public static Action RemoveIcon;

    private void OnEnable()
    {
        if (gameObject.tag == "Untagged")
        {
            gameObject.tag = "Interactible";
        }

        //dialogue is null, hasDialogue = false
        hasDialogue = dialogue != false;
    }

    //When the interactible is within Player's fow, triggered by fieldOfView.cs
    public void OnFocus()
    {
        if (MoveIconHere != null)
        {
            MoveIconHere(this.transform.position);
        }
    }

    //When the interactible is out of range for player, triggered by FieldOfView.cs
    public void OnDefocused()
    {
        if (RemoveIcon != null)
        {
            RemoveIcon();
        }
    }
}
