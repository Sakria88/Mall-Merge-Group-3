using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraScript : MonoBehaviour
{
    public float dampTime = 0.15f; // Time for the camera to catch up
    private Vector3 velocity = Vector3.zero; // Current velocity, this value is modified by SmoothDamp
    public Transform target; // The target for the camera to follow

    Vector2 destination;
    Vector2 destinationHolder;

    float camZ;
    
    void Start()
    {
        camZ = transform.position.z; // Store the initial Z position of the camera
        float camAspectNormalized = Mathf.InverseLerp(0.4f, 0.75f,
                                        GetComponent<Camera>().aspect); // Normalize aspect ratio

        GetComponent<Camera>().fieldOfView = Mathf.Lerp(90, 60, camAspectNormalized); // Adjust FOV based on aspect ratio

    }

    void Update()
    {
        if (target != null)
        {
            destination = target.position;

            if (destination.y < destinationHolder.y) // Only update destination if it's higher than the last recorded position
            {
                destination = destinationHolder;
            }
            else
            {
                destinationHolder = destination;
            }

            // Smoothly move the camera towards the target position
            transform.position = Vector3.SmoothDamp(transform.position,
                                              new Vector3(0, destination.y, camZ),
                                              ref velocity, dampTime);
        }
    }
}
