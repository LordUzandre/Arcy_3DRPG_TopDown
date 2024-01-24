using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEditor;

namespace Arcy.Animation
{
    public class NPCAnimationHandler : MonoBehaviour
    {
        //Public:
        [Header("Animator")]
        [SerializeField] public Animator animator;

        //private:
        [Header("IK")]
        [SerializeField] private Transform _aimController;
        [SerializeField] private MultiAimConstraint _multiAim;
        private Transform _aimTarget;
        private float _viewRadius = 5f;
        private float _viewAngle = 120f;
        private float _weight;
        private Vector3 _aimCtrlPosition = new Vector3(0, 0, 0);

        #region Components

        private void OnEnable()
        {
            CheckComponents();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            CheckComponents();
        }
#endif

        private void CheckComponents()
        {
            if (_aimController == null)
                foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(true))
                    if (transform.name == "aim-control")
                        _aimController = transform;

            if (_multiAim == null)
                foreach (MultiAimConstraint multiAimConstraint in gameObject.GetComponentsInChildren<MultiAimConstraint>(true))
                    if (multiAimConstraint.name == "Aim_head")
                        _multiAim = multiAimConstraint;

            if (_multiAim.data.constrainedObject == null)
                foreach (Transform bone in gameObject.GetComponentsInChildren<Transform>(true))
                    if (bone.name == "mixamorig:Neck")
                        _multiAim.data.constrainedObject = bone;

            if (animator == null)
            {
                if (TryGetComponent<Animator>(out Animator animFound))
                {
                    animator = animFound;
                }
                else
                {
                    Transform[] childObjects = new Transform[transform.childCount];

                    for (int i = 0; i < transform.childCount; i++)
                        childObjects[i] = transform.GetChild(i);

                    foreach (Transform childObject in childObjects)
                        if (childObject.TryGetComponent<Animator>(out Animator animFoundInChild))
                            animator = animFoundInChild;
                }
            }

            _aimTarget = GameObject.FindGameObjectWithTag("Player").transform;
        }

        #endregion

        private void Update()
        {
            if (_aimTarget != null)
            {
                if (Vector3.Distance(transform.position, _aimTarget.position) < _viewRadius)
                {
                    Vector3 dirToTarget = (_aimTarget.transform.position - transform.position).normalized;
                    float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

                    if (angleToTarget < (_viewAngle * .5f)) //is the player within the viewCone
                    {
                        _weight = 1;
                        _aimCtrlPosition = _aimTarget.position + Vector3.up;

                    }
                    else
                    {
                        _aimCtrlPosition = transform.position + transform.forward + new Vector3(0, 1.4f, 0);

                        _weight = 0;
                    }
                }

                _multiAim.weight = Mathf.Lerp(_multiAim.weight, _weight, .05f); //set weight of the anim constraint

                _aimController.position = Vector3.Lerp(_aimController.position, _aimCtrlPosition, .05f); //Set position of the aim-controller to player
            }
        }

        private void OnSceneGUI()
        {
            //Draw view cone
            Handles.color = Color.white;
            Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, _viewRadius);
            Vector3 viewAngleA = DirFromAngle(_viewAngle * .5f, false);
            Vector3 viewAngleB = DirFromAngle(_viewAngle * .5f, false);
            Handles.DrawLine(transform.position, transform.position + viewAngleA * _viewRadius);
            Handles.DrawLine(transform.position, transform.position + viewAngleB * _viewRadius);
        }

        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
                angleInDegrees += transform.eulerAngles.y;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}