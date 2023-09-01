using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] public float viewRadius = 4;
    [Range(0, 120)] public float viewAngle = 120;
    
    private PlayerManager playerManager;
    [HideInInspector] public bool multipleTargetsInView; //used by FieldOfViewEditor

    public LayerMask obstacleMask;

    [HideInInspector] public List<Interactible> visibleTargetsList = new List<Interactible>();
    [HideInInspector] public Interactible currentInteractible;
    private Interactible previousInteractible;

    void OnEnable()
    {
        StartCoroutine("FindTargetsWithDelay", .25f);
        playerManager = GetComponent<PlayerManager>();
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();

            //new interactible from the previous check
            if (currentInteractible != null && currentInteractible != previousInteractible)
            {
                currentInteractible.OnFocus();
                previousInteractible = currentInteractible;
            }

            //deactivate current Interactible
            if (currentInteractible == null && previousInteractible != null)
            {
                previousInteractible.OnDefocused();
                previousInteractible = null;
            }

            if (currentInteractible != null && playerManager != null)
            {
                playerManager.currentInteractible = currentInteractible;
            }            
        }
    }

    private void FindVisibleTargets()
    {
        currentInteractible = null;
        multipleTargetsInView = false;
        visibleTargetsList.Clear();
        Collider[] targetsInViewRadiusArray = Physics.OverlapSphere(transform.position, viewRadius);

        //No colliders in fow
        if (targetsInViewRadiusArray.Length == 0)
        {
            return;
        }

        foreach (Collider collider in targetsInViewRadiusArray)
        {
            Transform targetTransform = collider.transform;
            Vector3 dirToTarget = (targetTransform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

            //Is the target within viewcone
            if (angleToTarget < (viewAngle * .5f))
            {
                float dstToTarget = Vector3.Distance(transform.position, targetTransform.position);

                //is the target an interactible
                if (collider.TryGetComponent<Interactible>(out Interactible i))
                {
                    //Is the object blocked by obstacleLayer? (remove and replace)
                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                    {
                        visibleTargetsList.Add(i);
                        i.distanceToPlayer = dstToTarget;

                        switch (visibleTargetsList.Count)
                        {
                            case 0:
                                // no interactibles within fow
                                break;
                            case 1:
                                // 1 interactible in fow
                                currentInteractible = i;
                                break;
                            default:
                                // Multiple interactibles in fow
                                multipleTargetsInView = true;
                                if (angleToTarget < (viewAngle * .2f))
                                {
                                    if (i.distanceToPlayer < currentInteractible.distanceToPlayer)
                                    {
                                        //PossibleTarget is closer than closestTarget
                                        currentInteractible = i;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }
    }

    //used by FieldOfViewEditor
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}