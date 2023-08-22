using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    [SerializeField] public bool isNPC;
    [HideInInspector] public float distanceToPlayer;
    [HideInInspector] public GameObject standingPoint;
    [HideInInspector] public string speakerID;

    //UI
    private Vector3 originalFacingDirection;
    public static event System.Action<Vector3> MoveIconHere;
    public static event System.Action RemoveIcon;

    private void Start()
    {
        if (!isNPC)
        {
            //create a point for the player to stand and read.
            standingPoint = new GameObject("standingPlace");
            Instantiate(standingPoint, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - (8 * this.transform.localScale.z)), Quaternion.identity, this.transform);
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
        switch (isNPC)
        {
            case true:
                originalFacingDirection = this.transform.rotation.eulerAngles;
                StartCoroutine(RotateTowardsGoal(playerTransform));
                break;
            case false:
                break;
        }
    }

    //Have NPC rotate towards the player
    IEnumerator RotateTowardsGoal(Transform playerPosition)
    {
        while (true)
        {
            // Get the direction from this object to the target object
            Vector3 direction = playerPosition.position - transform.position;
            direction.y = 0f; // Set the y-component to zero to restrict rotation on y-axis only

            // Rotate this object to face the target object on y-axis
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * .5f);
            }
            else
            {
                StopCoroutine(RotateTowardsGoal(playerPosition));
            }
            yield return null;
        }
    }
}
