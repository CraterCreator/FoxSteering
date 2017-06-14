using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteeringBehaviour
{
    public Transform target;
    public float safeDistance = 100f;

    private Animator animator;
    public override Vector3 GetForce()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("moveSpeed", 6);

        Vector3 force = Vector3.zero;

        if (target == null)
        {
            return force;
        }
        
        Vector3 desiredForce = target.position + transform.position;
        
        desiredForce.y = 0;

        
        if (desiredForce.magnitude < safeDistance)
        {
            
            desiredForce = desiredForce.normalized * weighting;
            
            force = desiredForce - owner.velocity;
        }

        return force;
    }
}

