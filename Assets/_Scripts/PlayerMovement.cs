using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float turnSpeed = 180f;
    [SerializeField] float smoothFactor = 0.1f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 smoothVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDirection = transform.forward;

        inputReader.OnSwipe += SwipeDetected;
    }

    private void OnDisable()
    {
        inputReader.OnSwipe -= SwipeDetected;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Rotate towards the move direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
        }

        // Calculate the forward movement
        Vector3 targetVelocity = moveDirection * moveSpeed;

        // Smoothly interpolate the velocity
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref smoothVelocity, smoothFactor);
    }

    private void SwipeDetected(Vector2 swipeDelta)
    {
        // Convert 2D swipe to 3D direction
        Vector3 swipeDirection = new Vector3(swipeDelta.x, 0, swipeDelta.y).normalized;

        // Gradually change the move direction based on swipe
        moveDirection = Vector3.Lerp(moveDirection, swipeDirection, turnSpeed * Time.deltaTime);
        moveDirection.Normalize();
    }
}