using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputReader inputReader;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float turnSpeed = 180f;
    [SerializeField] Rigidbody rb;
    [SerializeField] float raycastDistance = 0.5f; // Adjust based on your player's size

    private Vector3 moveDirection;
    private bool canMove;

    public Action<Vector3> OnPositionUpdate;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveDirection = transform.forward;
        inputReader.OnSwipe += SwipeDetected;
        canMove = true;
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
        if (canMove)
        {
            // Rotate towards the move direction
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
            }

            // Calculate the new position
            Vector3 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

            // Check for wall collision
            if (!WillCollideWithWall(newPosition))
            {
                // Move the player
                rb.MovePosition(newPosition);
            }

            OnPositionUpdate?.Invoke(this.transform.position);
        }
    }

    private bool WillCollideWithWall(Vector3 newPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(rb.position, moveDirection, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                return true;
            }
        }
        return false;
    }

    private void SwipeDetected(Vector2 swipeDelta)
    {
        // Convert 2D swipe to 3D direction
        Vector3 swipeDirection = new Vector3(swipeDelta.x, 0, swipeDelta.y).normalized;
        // Gradually change the move direction based on swipe
        moveDirection = Vector3.Lerp(moveDirection, swipeDirection, turnSpeed * Time.deltaTime);
        moveDirection.Normalize();
    }

    public void UpdateMovebility(bool move)
    {
        canMove = move;
    }
}