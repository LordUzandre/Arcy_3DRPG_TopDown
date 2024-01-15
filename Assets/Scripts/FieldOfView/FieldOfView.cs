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
        public InteractibleBase currentInteractible;

        public static Action<Vector3> moveInteractionIconHere; //used by interactionIcon
        public static Action noObjectInFocus; //used by interactionIcon, also used in PlayerManager

        [HideInInspector]
        public bool multipleTargetsInView; //used by FieldOfViewEditor

        //private:
        private PlayerManager _playerManager;
        private InteractibleBase _previousInteractible;
        private float _previousInteractibleDistance;

        void OnEnable()
        {
            StartCoroutine(FindTargetsWithDelay());
            _playerManager = GetComponent<PlayerManager>();
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
                    _previousInteractible = currentInteractible;

                    if (moveInteractionIconHere != null && currentInteractible.isInteractible)
                    {
                        //calcuate interactionIcons new position

                        moveInteractionIconHere(currentInteractible.transform.position);
                    }

                    if (_playerManager != null)
                    {
                        _playerManager.currentInteractible = currentInteractible;
                        _playerManager.InteractibleNotNull(); //Subscribe to inputManager
                    }
                }

                //deactivate current Interactible
                if (currentInteractible == null && _previousInteractible != null)
                {
                    _previousInteractible = null;
                    _playerManager.currentInteractible = null;
                    _playerManager.UnSubscribeFromInteractible();

                    RemoveIcon();
                }

            }
        }

        public void RemoveIcon()
        {
            if (noObjectInFocus != null)
            {
                noObjectInFocus();
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
                                    currentInteractible = i;
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
}