using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public float nodeRadius = 1;
    public LayerMask unwalkableMask;
    public Node[,] nodes;

    private float nodeDiameter;
    private int gridSizeX, gridSizeZ;
    private Vector3 scale;
    private Vector3 halfScale;

    // Use this for initialization
    void Start()
    {
        CreateGrid();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(transform.position, transform.localScale);

        //Check if nodes have been created
        if (nodes != null)
        {
            // Loop through all the nodes
            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                for (int z = 0; z < nodes.GetLength(1); z++)
                {
                    // Get the node and store it in a variable
                    Node node = nodes[x, z];

                    // set colour of Gizmos for node depending on walkable
                    Gizmos.color = node.walkable ? new Color(0, 0, 1, 0.5f) : new Color(1, 0, 0, 0.5f);

                    // Draw a sphere to represent node
                    Gizmos.DrawSphere(node.position, nodeRadius);
                }
            }
        }
    }

    // Generates a 2d grid on the x and z axis
    public void CreateGrid()
    {
        // Calculate the node diameter
        nodeDiameter = nodeRadius * 2f;

        // get transforms scale
        scale = transform.localScale;

        // Half the scale
        halfScale = scale / 2;

        // Calcualte the grid size in int form
        gridSizeX = Mathf.RoundToInt(scale.x / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(scale.z / nodeDiameter);

        //Create 2d array of grid sizes calculated
        nodes = new Node[gridSizeX, gridSizeZ];

        // Get the bottom left point of the graph
        Vector3 bottomLeft = transform.position - Vector3.right * halfScale.x - Vector3.forward * halfScale.z;

        // loop through all the nodes in the grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                // Calculate an offset for x and z
                float xOffset = x * nodeDiameter + nodeRadius;
                float zOffset = z * nodeDiameter + nodeRadius;
                // Create position using offsets
                Vector3 nodePos = bottomLeft + Vector3.right * xOffset + Vector3.forward * zOffset;

                // Use physics to check is node collides with non-walkable object
                bool walkable = !Physics.CheckSphere(nodePos, nodeRadius, unwalkableMask);
                //Create node and place in 2d array coordinate
                nodes[x, z] = new Node(walkable, nodePos, x, z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckWalkables();
    }

    public void CheckWalkables()
    {
        //Loop through all the nodes
        for (int x = 0; x < nodes.GetLength(0); x++)
        {
            for (int z = 0; z < nodes.GetLength(1); z++)
            {
                // Grab node at index
                Node node = nodes[x, z];
                // Check if node collided with unwalkable
                node.walkable = !Physics.CheckSphere(node.position, nodeRadius, unwalkableMask);
            }
        }
    }

    public Node GetNodeFromPosition(Vector3 position)
    {
        // Calculate percentage of grid position
        float percentX = (position.x + halfScale.x) / scale.x;
        float percentZ = (position.z + halfScale.z) / scale.z;

        // Clamp percentage to a 0-1 ratio
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);

        Node node = nodes[x, z];

        if (!node.walkable)
            return FindClosestWalkable(node);

        //Return the node at translated coordinate
        return node;


    }

    public Node FindClosestWalkable(Node node)
    {
        for (int i = 0; i < gridSizeX * gridSizeZ; i++)
        {
            List<Node> neighbours = new List<Node>();
            neighbours = GetNeighbours(node);
            foreach (Node neighbour in neighbours)
            {
                if (neighbour.walkable)
                    return neighbour;
            }
        }
        return null;
    }

    public List<Node> GetNeighbours(Node node)
    {
        // Make new list of neighbours
        List<Node> neighbours = new List<Node>();
        // try and look at surrounding neighbours
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                // if the current index is the node that is being checked
                if (x == 0 && z == 0)
                    continue;

                //Check the current surrounding node
                int checkX = node.gridX + x;
                int checkZ = node.gridZ + z;

                // Check if the index is in the bounds of the grid
                if (checkX >= 0 &&
                    checkX < gridSizeX &&
                    checkZ >= 0 &&
                    checkZ < gridSizeZ)
                {
                    // add the neighbour to the list
                    neighbours.Add(nodes[checkX, checkZ]);
                }
            }

        }
        // Return the neighbours
        return neighbours;
    }
}
