using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NpcLookAt : MonoBehaviour
{
    [SerializeField] private Transform aimController;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private MultiAimConstraint multiAim;
    [SerializeField] private float viewRadius = 5;
    [SerializeField] private float viewAngle = 120;
    private float delay = .1f;

    private void OnEnable()
    {
        StartCoroutine(Fow());
    }

    private void OnDisable()
    {
        StopCoroutine(Fow());
    }

    IEnumerator Fow()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindPlayer();

            float weight = (aimTarget == null) ? 0 : 1f;
            Vector3 thisPos = (aimTarget == null) ? transform.position + transform.forward + new Vector3(0, 1.2f, 0) : aimTarget.position + Vector3.up;

            multiAim.weight = Mathf.Lerp(multiAim.weight, weight, .05f); //how "much" is the NPC is the looking?
            aimController.position = Vector3.Lerp(aimController.position, thisPos, .05f); //"where" is the character looking
        }
    }

    private void FindPlayer()
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
