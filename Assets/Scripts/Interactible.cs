using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = System.Action;

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
    public static event Action RemoveIcon;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            isNPC = false;
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

    public void InteractStarted()
    {
        if (haveDialogue)
        {
            DialogueManager.instance.StartDialogue(dialogue);
        }
    }
}
