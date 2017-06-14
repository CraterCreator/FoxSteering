using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringBehaviour
{
    public Transform target;
    public float stoppingDistance = 0f;

    private Animator animator;

    public override Vector3 GetForce()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("moveSpeed", 6);

        Vector3 force = Vector3.zero;

        // IF there is no target, then return force
        if(target == null)
        {
            return force;
        }
        // SET desiredForce to direction from target to position
        Vector3 desiredForce = target.position - transform.position;
        // SET desiredForce y to zero
        desiredForce.y = 0;

        // IF direction is greater than stopping distance
        if(desiredForce.magnitude > stoppingDistance)
        {
            // SET desired force to normalized and multiply by weighting
            desiredForce = desiredForce.normalized * weighting;
            // SET force to desiredForce and subtract owners velocity
            force = desiredForce - owner.velocity;
        }

        return force;
    }
}
