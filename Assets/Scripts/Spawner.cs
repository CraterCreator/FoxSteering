using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float spawnRate = 1f;
    public List<GameObject> objects = new List<GameObject>();

    private float spawnerTimer = 0f;
    
    
    void OnDrawGizmos()
    {
        // Draw a cube to indicate where the box is that we're spawning objects
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    // Generates a random point within the transforms scale
    Vector3 GenerateRandomPoint()
    {
        // Set halfscale to half of the transforms scale
        Vector3 halfScale = transform.localScale / 2;
        // Set randomPoint vector to zero
        Vector3 randomPoint = Vector3.zero;
        // Set randompoint x, y, z to random range between
            // -halfscale to halfscale (can do individually)
        randomPoint.x = Random.Range(-halfScale.x, halfScale.x);
        randomPoint.y = Random.Range(-halfScale.y, halfScale.y);
        randomPoint.z = Random.Range(-halfScale.z, halfScale.z);
        // Return randomPoint
        return randomPoint;
    }

    // Spawns the prefab at a given position with rotation
    public void Spawn(Vector3 position, Quaternion rotation)
    {
        // Set clone to new instance of prefab
        GameObject clone = Instantiate(prefab);
        // Add clone to objects list
        objects.Add(clone);
        // Set clones position to spawner position + position
        clone.transform.position = position;
        // Set clones rotation to rotation
        clone.transform.rotation = rotation;
    }

    void Update()
    {
        //Set spawnTimer to spawnTimer + delta time
        spawnerTimer = spawnerTimer + Time.deltaTime;
        // If spawnTimer > spawnRate
        if(spawnerTimer > spawnRate)
        {
            // Set randomPoint to GenerateRandomPoint
            Vector3 randomPoint = GenerateRandomPoint();
            // Call Spawn() and pass randomPoint, Quaternion identity
            Spawn(transform.position + randomPoint, Quaternion.identity);
            // Set spawnTimer to zero
            spawnerTimer = 0;
        }
    }
}
