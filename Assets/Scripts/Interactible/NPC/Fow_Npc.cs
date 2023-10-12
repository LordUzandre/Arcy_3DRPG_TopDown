using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Fow_Npc : MonoBehaviour
{
    [SerializeField] private Transform aimController;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private MultiAimConstraint multiAim;
    [SerializeField] private float viewRadius = 5;
    [SerializeField] private float viewAngle = 120;
    private float delay = .05f;

    private void Start()
    {
        #region Check Components
        if (aimController == null)
        {
            foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(true))
            {
                if (transform.name == "aim-control")
                {
                    aimController = transform;
                }
            }
        }

        if (multiAim == null)
        {
            foreach (MultiAimConstraint multiAimConstraint in gameObject.GetComponentsInChildren<MultiAimConstraint>(true))
            {
                if (multiAimConstraint.name == "Aim_head")
                {
                    multiAim = multiAimConstraint;
                }
            }
        }

        if (multiAim.data.constrainedObject == null)
        {
            foreach (Transform bone in gameObject.GetComponentsInChildren<Transform>(true))
            {
                if (bone.name == "mixamorig:Neck")
                {
                    multiAim.data.constrainedObject = bone;
                }
            }
        }
        #endregion
    }

    private void Update()
    {
        if (true)
        {
            FindPlayer();

            float weight = (aimTarget == null) ? 0 : 1f;
            Vector3 thisPos = (aimTarget == null) ? transform.position + transform.forward + new Vector3(0, 1.2f, 0) : aimTarget.position + Vector3.up;

            //set weight of the anim constraint
            multiAim.weight = Mathf.Lerp(multiAim.weight, weight, .05f);
            //Set position of the aim-controller to player
            aimController.position = Vector3.Lerp(aimController.position, thisPos, .05f);

        }
    }

    private void FindPlayer() //SphereCast, checking for player by tag
    {
        aimTarget = null;
        Collider[] targetsInViewRadiusArray = Physics.OverlapSphere(transform.position, viewRadius);

        //No colliders in fow
        if (targetsInViewRadiusArray.Length == 0) return;

        foreach (Collider other in targetsInViewRadiusArray)
        {
            if (other.CompareTag("Player"))
            {
                Vector3 dirToTarget = (other.transform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

                if (angleToTarget < (viewAngle * .5f)) //is the player within the viewCone
                {
                    aimTarget = other.transform;
                    delay = Time.deltaTime;
                }
            }
        }

        if (aimTarget == null) delay = .1f;
    }
}
