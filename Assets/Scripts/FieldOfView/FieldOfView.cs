// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Arcy.Player;
using Arcy.Management;

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
        public List<IInteractibleBase> visibleTargetsList = new List<IInteractibleBase>();
        [HideInInspector]
        public IInteractibleBase currentInteractible;

        [HideInInspector]
        public bool multipleTargetsInView; //used by FieldOfViewEditor

        //private:
        private PlayerManager _playerManager;
        private IInteractibleBase _previousInteractible;
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

                    if (currentInteractible.isInteractible)
                    {
                        //calcuate interactionIcons new position
                        GameManager.instance.gameEventManager.interactionEvents.onMoveInteractionIconHere(currentInteractible.ObjectTransform.position);
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
            GameManager.instance.gameEventManager.interactionEvents.noObjectInFocus();
        }

        /// <summary> 
        /// When there are multiple targets in fow. (MultipleTartgetsInView is used by fow.Editor)
        /// First it narrows the field of search, and if there are no interactibles in the new cone there will be no currentInteractible.
        /// It then chooses the one closest to player.
        /// </summary>

        private void FindVisibleTargets()
        {
            currentInteractible = null;
            multipleTargetsInView = false;
            visibleTargetsList.Clear();
            Collider[] targetsInViewRadiusArray = Physics.OverlapSphere(transform.position, viewRadius);

            //No colliders in fow
            if (targetsInViewRadiusArray.Length == 0)
                return;

            foreach (Collider collider in targetsInViewRadiusArray)
            {
                Transform targetTransform = collider.transform;
                Vector3 dirToTarget = (targetTransform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

                //Is the target within viewcone
                if (angleToTarget < (viewAngle * .5f))
                {
                    //is the target an interactible
                    if (collider.TryGetComponent(typeof(IInteractibleBase), out Component interactibleBase))
                    {
                        float dstToTarget = Vector3.Distance(transform.position, targetTransform.position);

                        //Is the object blocked by obstacleLayer? (remove and replace with different system?)
                        if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                        {
                            IInteractibleBase i = interactibleBase as IInteractibleBase;
                            visibleTargetsList.Add(i);

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
                                    multipleTargetsInView = true;

                                    if (angleToTarget < (viewAngle * .2f))
                                        if (dstToTarget < _previousInteractibleDistance)
                                            currentInteractible = i;
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

#if UNITY_EDITOR
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : Editor
    {
        private void OnSceneGUI()
        {
            FieldOfView fow = (FieldOfView)target;
            Vector3 pos = new Vector3(fow.transform.position.x, fow.transform.position.y + 1, fow.transform.position.z);

            //Draw view cone
            Handles.color = Color.white;

            Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle * .5f, false);
            Handles.DrawLine(pos, pos + viewAngleA * fow.viewRadius);
            Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle * .5f, false);
            Handles.DrawLine(pos, pos + viewAngleB * fow.viewRadius);

            Handles.DrawWireArc(pos, Vector3.up, viewAngleA, fow.viewAngle, fow.viewRadius);
            //Draw line to current interactible
            if (fow.multipleTargetsInView)
            {
                foreach (IInteractibleBase i in fow.visibleTargetsList)
                {
                    if (i == fow.currentInteractible) Handles.color = Color.red;
                    else Handles.color = Color.white;

                    Handles.DrawLine(fow.transform.position, fow.currentInteractible.ObjectTransform.position);
                }
            }
            else if (fow.currentInteractible != null)
            {
                Handles.color = Color.red;
                Handles.DrawLine(fow.transform.position, fow.currentInteractible.ObjectTransform.position);
            }
        }
    }
#endif
}