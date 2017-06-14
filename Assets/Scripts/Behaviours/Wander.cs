using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GGL;

public class Wander : SteeringBehaviour
{
    public float offset = 1.0f;
    public float radius = 1.0f;
    public float jitter = 0.2f;

    private Vector3 targetDir;
    private Vector3 randomDir;
    private Animator animator;


    public override Vector3 GetForce()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("moveSpeed", 1);

        // Set force to 0
        Vector3 force = Vector3.zero;
        // Generate random numbers between a certain range

        // 0 x 7fff = 32767
        float randX = Random.Range(0, 0x7fff) - (0x7fff * 0.5f);
        float randZ = Random.Range(0, 0x7fff) - (0x7fff * 0.5f);

        #region Calculate RandomDir
        // Set randomDir to new Vector3 x = randX & z = randZ
        randomDir = new Vector3(randX, 0, randZ);
        // Set randomDir to normalized randomDir
        randomDir = randomDir.normalized;
        // Set randomDir to randomDir x jitter (amplify randomDir by jitter)
        randomDir = randomDir * jitter;
        #endregion

        #region Calculate TargetDir
        // Set targetDir to targetDir + randomDir
        targetDir = targetDir + randomDir;
        // Set targetDir to normalized targetDir
        targetDir = targetDir.normalized;
        // Set targetDir to targetDir x radius
        targetDir = targetDir * radius;
        #endregion

        #region Calculate Force
        // Set seekPos to owners position + targetDir
        Vector3 seekPos = owner.transform.position + targetDir;
        // Set seekPos to seekPos + owners forward x offset
        seekPos = seekPos + owner.transform.forward * offset;

        #region GIZMOS
        // Set offsetPos to position + forward x offset
        Vector3 offsetPos = transform.position + transform.forward.normalized * offset;
        // Add circle with offsetPos + up x amount, rotate circle with LookRotation (down)
        GizmosGL.AddCircle(offsetPos + Vector3.up * 0.1f, radius, Quaternion.LookRotation(Vector3.down), 16, Color.red);
        GizmosGL.AddCircle(seekPos + Vector3.up * 0.15f, radius * 0.6f, Quaternion.LookRotation(Vector3.down), 16, Color.blue);
        #endregion

        // Set desiredForce to seekPos - position
        Vector3 desiredForce = seekPos - transform.position;
        // IF desired force is not zero
        if (desiredForce.magnitude > 0)
        {
            // Set desiredForce to desiredForce normalized x weighting
            desiredForce = desiredForce.normalized * weighting;
            // Set force to desiredForce - owners velocity
            force = desiredForce - owner.velocity;
        }
        #endregion
        // Return force
        return force;
    }
}
