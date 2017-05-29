using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target; // Target to orbit around
    public float distance = 5f;
    public float orthoSize = 5f;
    public float zoomSpeed = 5f;

    public float xSpeed = 120f;
    public float ySpeed = 120f;

    public float yMinLimit = 0;
    public float yMaxLimit = 80f;

    public float minDistance = 5f;
    public float maxDistance = 20f;

    public float minOrthoSize = 5f;
    public float maxOrthoSize = 20f;

    private float x = 0; // Pitch
    private float y = 0; // Yaw

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        // Check if camera is orthographic
        if (Camera.main.orthographic)
        {
            orthoSize = Mathf.Clamp(orthoSize -= scroll, minOrthoSize, maxOrthoSize);
        }
        else
        {
            distance = Mathf.Clamp(distance - scroll, minDistance, maxDistance);
        }

        // Check if there is a target and right mouse button is pressed
        if (target != null)
        {
            if (Input.GetMouseButton(1))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                // Rotate camera based off mouse coordinates
                x += -mouseY * ySpeed * Time.deltaTime;
                y += mouseX * xSpeed * Time.deltaTime;
            }

            // Create quaternion to store new rotations
            Quaternion rotation = Quaternion.Euler(x, y, 0);
            Vector3 negativeDistance = new Vector3(0, 0, -distance);
            Vector3 position = rotation * negativeDistance + target.position;
            // Apply those calculations
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}
