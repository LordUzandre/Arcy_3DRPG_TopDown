using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = System.Action;

public class Interactible : MonoBehaviour
{
    [SerializeField] public bool isNPC;
    [SerializeField] public bool haveDialogue;
    [HideInInspector] public float distanceToPlayer;
    [HideInInspector] public string speakerID;
    [HideInInspector] public Animator animator;
    public Dialogue dialogue;

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
    public void OnFocus()
    {
        if (MoveIconHere != null)
        {
            MoveIconHere(this.transform.position);
        }
    }

    public void OnDefocused()
    {
        if (RemoveIcon != null)
        {
            RemoveIcon();
            //StartCoroutine(RotateTowardsGoal(originalFacingDirection));
        }
    }

    //event triggered by
    public void InteractionStarted(Transform playerTransform)
    {
        
    }
}
