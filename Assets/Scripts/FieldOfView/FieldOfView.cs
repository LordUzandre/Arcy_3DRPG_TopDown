using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    //public:
    [SerializeField] public float viewRadius = 4;
    [Range(0, 120)] public float viewAngle = 120;
    [SerializeField] public LayerMask obstacleMask;
    [HideInInspector] public List<Interactible> visibleTargetsList = new List<Interactible>();
    [HideInInspector] public Interactible currentInteractible;
    [HideInInspector] public bool multipleTargetsInView; //used by FieldOfViewEditor

    //private:
    private PlayerManager _playerManager;
    private Interactible _previousInteractible;
    private GameObject _interactionIcon;

    void OnEnable()
    {
        StartCoroutine(FindTargetsWithDelay());
        _playerManager = GetComponent<PlayerManager>();
        _interactionIcon = GameObject.FindGameObjectWithTag("InteractionIcon");
    }

    IEnumerator FindTargetsWithDelay()
    {
        WaitForSeconds fowDelay = new WaitForSeconds(.25f);

        while (true)
        {
            yield return fowDelay;
            FindVisibleTargets();

            //new interactible from the previous check
            if (currentInteractible != null && currentInteractible != _previousInteractible)
            {
                currentInteractible.OnFocus();
                _previousInteractible = currentInteractible;
            }

            //deactivate current Interactible
            if (currentInteractible == null && _previousInteractible != null)
            {
                _previousInteractible.OnDefocused();
                _previousInteractible = null;
            }

            if (_playerManager != null)
            {
                _playerManager.currentInteractible = currentInteractible;
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
                                MoveIconToInteractible(i.transform.position);
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
                                        MoveIconToInteractible(i.transform.position);
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }
    }

    public void MoveIconToInteractible(Vector3 interactiblePosition)
    {
        _interactionIcon.transform.position = interactiblePosition;
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