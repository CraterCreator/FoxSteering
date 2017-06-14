using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGL;

public class PathFollowing : SteeringBehaviour
{
    public Transform target;
    // Distance to current node
    public float nodeRadius = 5f;
    // Distance to end node
    public float targetRadius = 3f;

    private Graph graph;
    private int currentNode = 0;
    private bool isAtTarget = false;
    private List<Node> path;
    // Use this for initialization
    void Start()
    {
        // SET graph to FindObjectOfType Graph
        graph = FindObjectOfType<Graph>();
        // IF graph is null
        if (graph == null)
        {
            // CALL Debug.LogError() and pass an error message
            Debug.LogError("Error!");
            // CALL Debug.Break() (pauses the editor)
            Debug.Break();
        }
    }

    // Updates list of nodes for path following
    public void UpdatePath()
    {
        // SET path to graph.FindPath() and pass transform's position, target's position
        path = graph.FindPath(transform.position, target.position);
        // SET current node to zero
        currentNode = 0;
    }

    #region Seek
    // Special version of seek that takes into account the node radius and target radius
    Vector3 Seek(Vector3 target)
    {
        // SET force to zero
        Vector3 force = Vector3.zero;
        // SET desiredForce to target - transform's position
        Vector3 desiredForce = target - transform.position;
        // SET desiredForce.y to zero
        desiredForce.y = 0;

        // SET distance to zero
        // IF isAtTarget is true. NOTE: this needs to be done in a ternary operator
        // SET distance to targetRadius
        // Else
        // SET distance to nodeRadius
        float distance = isAtTarget ? targetRadius : nodeRadius;

        // IF desiredForce's length is greater than distance
        if (desiredForce.magnitude > distance)
        {
            // SET desiredForce to desiredForce.normalized * weighting
            desiredForce = desiredForce.normalized * weighting;
            // SET force to desiredForce - owners velocity
            force = desiredForce - owner.velocity;
        }

        // RETURN force
        return force;

    }
    #endregion

    #region GetForce
    // Calculates force for behaviour
    public override Vector3 GetForce()
    {
        // SET force to zero
        Vector3 force = Vector3.zero;

        // IF path is not null AND path count is greater than zero
        if (path != null && path.Count > 0)
        {
            // SET currentPos to path[currentNode] position
            Vector3 currentPos = path[currentNode].position;
            // IF distance between transform's position and currentPos is less or equal to nodeRadius
            float distance = Vector3.Distance(transform.position, currentPos);
            if (distance <= nodeRadius)
            {
                // Increment currentNode
                currentNode++;
                // IF currentNode is greater than or equal to path.Count
                if (currentNode >= path.Count)
                {
                    // SET currentNode to path.Count - 1
                    currentNode = path.Count - 1;
                }
            }

            // SET force to Seek() and pass position
            force = Seek(currentPos);
            #region GIZMOS
            // SET prevPosition to path[0].position
            Vector3 prevPosition = path[0].position;
            // FOREACH node in path 
            foreach (var node in path)
            {
                // CALL GizmosGL.AddSphere() and pass node's position, graph's nodeRadius, identity, and color
                GizmosGL.AddSphere(node.position, graph.nodeRadius, Quaternion.identity, Color.red);
                // CALL  GizmosGL.AddLine() and pass prev, nodes position, 0.1f, 0.1f, any color, any color
                GizmosGL.AddLine(prevPosition, node.position, 0.1f, 0.1f, Color.black, Color.red);
                // SET prev to node's position
                prevPosition = node.position;

            }
            #endregion
        }

        // RETURN force
        return force;

    }
    #endregion
}
