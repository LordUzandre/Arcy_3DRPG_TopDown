using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arcy.Interaction
{
    public class FieldOfView : MonoBehaviour
    {
        //Public
        [SerializeField] public float viewRadius = 4;
        [Range(0, 120)] public float viewAngle = 120;
        [SerializeField] private LayerMask obstacleMask;

        [HideInInspector] public List<IInteractible> visibleTargetsList = new List<IInteractible>();
        [HideInInspector] public IInteractible currentInteractible;
        [HideInInspector] public bool multipleTargetsInView; //used by FieldOfViewEditor

        // Private:
        private IInteractible _previousInteractible;
        private PlayerManager _playerManager;
        private GameObject _interactionIcon;
        private Vector3 _iconOffset = new Vector3(0, 2f, -0.25f);
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.2f);
        private float _currentInteractibleDstToPlayer;

        void OnEnable()
        {
            StartCoroutine("FindTargetsWithDelay", .25f);
            _playerManager = GetComponent<PlayerManager>();

            if (_interactionIcon == null)
            {
                _interactionIcon = GameObject.FindGameObjectWithTag("InteractionIcon");
            }
        }

        private void Reset()
        {
            _interactionIcon = GameObject.FindGameObjectWithTag("InteractionIcon");
        }

        IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return _waitForSeconds;
                FindVisibleTargets();

                //new interactible from the previous check
                if (currentInteractible != null && currentInteractible != _previousInteractible)
                {
                    //currentInteractible.OnFocus();
                    MoveIconToInteractible(currentInteractible.transform.position);
                    _previousInteractible = currentInteractible;
                }

                //deactivate current Interactible
                if (currentInteractible == null && _previousInteractible != null)
                {
                    //_previousInteractible.OnDefocused();
                    _previousInteractible = null;
                }

                if (_playerManager != null && currentInteractible != null)
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
                //variables
                Transform targetTransform = collider.transform;
                Vector3 dirToTarget = (targetTransform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

                //Is the target within viewcone
                if (angleToTarget < (viewAngle * .5f))
                {
                    //is the target an interactible
                    if (collider.TryGetComponent<IInteractible>(out IInteractible i))
                    {
                        float dstToTarget = Vector3.Distance(transform.position, targetTransform.position);

                        //Is the object blocked by obstacleLayer? (remove and replace)
                        if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                        {
                            visibleTargetsList.Add(i);
                            float distanceToPlayer = dstToTarget;

                            switch (visibleTargetsList.Count)
                            {
                                case 0: // no interactibles within fow
                                    break;
                                case 1: // 1 interactible in fow
                                    currentInteractible = i;
                                    break;
                                default: // Multiple interactibles in fow
                                    multipleTargetsInView = true;
                                    if (angleToTarget < (viewAngle * .2f))
                                    {
                                        if (distanceToPlayer < _currentInteractibleDstToPlayer)
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

        public void MoveIconToInteractible(Vector3 interactiblePosition)
        {
            if (_interactionIcon != null)
            {
                _interactionIcon.transform.position = interactiblePosition + _iconOffset;
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
}