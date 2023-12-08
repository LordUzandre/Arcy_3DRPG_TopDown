using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Interaction
{
    public class FieldOfView : MonoBehaviour
    {
        //public:
        [SerializeField]
        public float viewRadius = 4;
        [Range(0, 120)]
        public float viewAngle = 120;
        [SerializeField]
        public LayerMask obstacleMask;
        [HideInInspector]
        public List<InteractibleBase> visibleTargetsList = new List<InteractibleBase>();
        [HideInInspector]
        public InteractibleBase currentInteractibleBase;
        // [HideInInspector] public Interactible currentInteractible;
        public static Action<Vector3> moveInteractionIconHere;
        public static Action noObjectInFocus;

        //used by FieldOfViewEditor
        [HideInInspector]
        public bool multipleTargetsInView;

        //private:
        private PlayerManager _playerManager;
        private InteractibleBase _previousInteractible;
        //private GameObject _interactionIcon;
        private float _previousInteractibleDistance;

        void OnEnable()
        {
            StartCoroutine(FindTargetsWithDelay());

            _playerManager = GetComponent<PlayerManager>();
            //_interactionIcon = GameObject.FindGameObjectWithTag("InteractionIcon");
        }

        IEnumerator FindTargetsWithDelay()
        {
            WaitForSeconds fowDelay = new WaitForSeconds(.25f);

            while (true)
            {
                yield return fowDelay;
                FindVisibleTargets();

                //new interactible from the previous check
                if (currentInteractibleBase != null && currentInteractibleBase != _previousInteractible)
                {
                    _previousInteractible = currentInteractibleBase;
                    
                    if (moveInteractionIconHere != null)
                    {
                        moveInteractionIconHere(currentInteractibleBase.transform.position);
                    }

                    if (_playerManager != null)
                    {
                        _playerManager.currentInteractible = currentInteractibleBase;
                    }
                }

                //deactivate current Interactible
                if (currentInteractibleBase == null && _previousInteractible != null)
                {
                    //_previousInteractible.OnDefocused();
                    _previousInteractible = null;

                    if (noObjectInFocus != null)
                    {
                        noObjectInFocus();
                    }
                }

            }
        }

        private void FindVisibleTargets()
        {
            currentInteractibleBase = null;
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
                    //is the target an interactible
                    if (collider.TryGetComponent(typeof(InteractibleBase), out Component component))
                    {
                        float dstToTarget = Vector3.Distance(transform.position, targetTransform.position);

                        //Is the object blocked by obstacleLayer? (remove and replace)
                        if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                        {
                            InteractibleBase i = component as InteractibleBase;
                            visibleTargetsList.Add(i);

                            // go through the list iof interactibles
                            switch (visibleTargetsList.Count)
                            {
                                case 0:
                                    // no interactibles within fow
                                    break;
                                case 1:
                                    // 1 interactible in fow
                                    currentInteractibleBase = i;
                                    break;
                                default:

                                    /*
	                                When there are multiple targets in fow. (MultipleTartgetsInView is used by fow.Editor)
	                                First it narrows the field of search, and if there are no interactibles in the new cone there will be no currentInteractible.
	                                It then chooses the one closest to player.
	                                */

                                    multipleTargetsInView = true;

                                    if (angleToTarget < (viewAngle * .2f))
                                    {
                                        if (dstToTarget < _previousInteractibleDistance)
                                        {
                                            currentInteractibleBase = i;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        // public void MoveIconToInteractible(Vector3 interactiblePosition)
        // {
        //     _interactionIcon.transform.position = interactiblePosition;
        // }

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