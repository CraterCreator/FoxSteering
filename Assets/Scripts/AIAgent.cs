using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public Vector3 force;
    public Vector3 velocity;
    public float maxVelocity = 100f;

    private List<SteeringBehaviour> behaviours;
    // Use this for initialization
    void Start()
    {
        SteeringBehaviour[] behaviourArray = GetComponents<SteeringBehaviour>();
        behaviours = new List<SteeringBehaviour>(behaviourArray);
    }

    // Update is called once per frame
    void Update()
    {
        ComputeForces();
        ApplyVelocity();
    }

    void ComputeForces()
    {
        // Reset the force before computing
        force = Vector3.zero;

        // FOR each behaviour attached to AIAgent
        for (int i = 0; i < behaviours.Count; i++)
        {
            SteeringBehaviour behaviour = behaviours[i];
            // Check if behaviour is not active
            if (!behaviour.enabled)
            {
                // CONTINUE to the next function
                continue;
            }
            // Get force from behaviour (Multiply by behaviours weighting)
            force += behaviour.GetForce();
            //IF forces are too big 
            if (force.magnitude >= maxVelocity)
            {
                //clamp force to max velocity
                force = force.normalized * maxVelocity;
                // EXIT for loop
                break;
            }
        }
    }

    void ApplyVelocity()
    {
        // Append force to velocity with deltaTime
        velocity += force * Time.deltaTime;
        // IF velocity is greater than max vel
        if (velocity.magnitude >= maxVelocity)
        {
            // Clamp velocity
            velocity = velocity.normalized * maxVelocity;
        }
        // If velocity is not zero
        if(velocity.magnitude > 0)
        {
            // Append transform position by velocity
            transform.position += velocity * Time.deltaTime;
            // transform rotate by velocity
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
}
