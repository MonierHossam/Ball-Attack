using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;  // The player's transform
    [SerializeField] float distance = 10.0f;  // Distance from the target
    [SerializeField] float height = 5.0f;  // Height above the target
    [SerializeField] float smoothSpeed = 0.125f;  // Smoothing factor for camera movement
    [SerializeField] Vector3 offset = new Vector3(0, 0, -2);  // Offset from the target's position

    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target not set!");
            return;
        }

        // Calculate the desired position
        desiredPosition = target.position - target.forward * distance + Vector3.up * height;

        // Add offset
        desiredPosition += target.TransformDirection(offset);

        // Smoothly move the camera towards the desired position
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Make the camera look at the target
        transform.LookAt(target.position + target.forward * 2);
    }
}