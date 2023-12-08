using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Arcy.Animation
{
    public class NPCAnimationHandler : MonoBehaviour
    {
        [SerializeField] private Transform _aimController;
        [SerializeField] private Transform _aimTarget;
        [SerializeField] private MultiAimConstraint _multiAim;
        [SerializeField] private Animator _anim;
        
        private float _viewRadius = 5f;
        private float _viewAngle = 120f;
        private float _delay = .05f;

        private void OnEnable()
        {
            CheckComponents();
        }

        private void Reset()
        {
            CheckComponents();
        }

        private void CheckComponents()
        {
            if (_aimController == null)
            {
                foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(true))
                {
                    if (transform.name == "aim-control")
                    {
                        _aimController = transform;
                    }
                }
            }

            if (_multiAim == null)
            {
                foreach (MultiAimConstraint multiAimConstraint in gameObject.GetComponentsInChildren<MultiAimConstraint>(true))
                {
                    if (multiAimConstraint.name == "Aim_head")
                    {
                        _multiAim = multiAimConstraint;
                    }
                }
            }

            if (_multiAim.data.constrainedObject == null)
            {
                foreach (Transform bone in gameObject.GetComponentsInChildren<Transform>(true))
                {
                    if (bone.name == "mixamorig:Neck")
                    {
                        _multiAim.data.constrainedObject = bone;
                    }
                }
            }
        }

        private void Update()
        {
            if (true)
            {
                FindPlayer();

                float weight = (_aimTarget == null) ? 0 : 1f;
                Vector3 thisPos = (_aimTarget == null) ? transform.position + transform.forward + new Vector3(0, 1.2f, 0) : _aimTarget.position + Vector3.up;

                //set weight of the anim constraint
                _multiAim.weight = Mathf.Lerp(_multiAim.weight, weight, .05f);
                //Set position of the aim-controller to player
                _aimController.position = Vector3.Lerp(_aimController.position, thisPos, .05f);

            }
        }

        private void FindPlayer() //SphereCast, checking for player by tag
        {
            _aimTarget = null;
            Collider[] targetsInViewRadiusArray = Physics.OverlapSphere(transform.position, _viewRadius);

            //No colliders in fow
            if (targetsInViewRadiusArray.Length == 0) return;

            foreach (Collider other in targetsInViewRadiusArray)
            {
                if (other.CompareTag("Player"))
                {
                    Vector3 dirToTarget = (other.transform.position - transform.position).normalized;
                    float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

                    if (angleToTarget < (_viewAngle * .5f)) //is the player within the viewCone
                    {
                        _aimTarget = other.transform;
                        _delay = Time.deltaTime;
                    }
                }
            }

            if (_aimTarget == null) _delay = .1f;
        }
    }
}
