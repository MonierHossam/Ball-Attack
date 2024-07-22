using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Input Reader", menuName = "Monier/New Input Reader")]
public class InputReader : ScriptableObject
{
    private UserInput userInput;
    private Vector2 inputDeltaPosition;
    public float swipeThreshold = 50f;

    public event Action<Vector2> OnSwipe;

    private void OnEnable()
    {
        userInput = new UserInput();

        userInput.GamePlay.Enable();
        userInput.GamePlay.MouseMove.performed += OnMouseMove;
        userInput.GamePlay.TouchMove.performed += OnTouchMove;
    }

    private void OnDisable()
    {
        userInput.GamePlay.Disable();
        userInput.GamePlay.MouseMove.performed -= OnMouseMove;
        userInput.GamePlay.TouchMove.performed -= OnTouchMove;
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        inputDeltaPosition = context.ReadValue<Vector2>();

        OnSwipe?.Invoke(inputDeltaPosition);
    }

    private void OnTouchMove(InputAction.CallbackContext context)
    {
        inputDeltaPosition = context.ReadValue<Vector2>();

        OnSwipe?.Invoke(inputDeltaPosition);
    }
}

public enum SwipeDirection
{
    Left,
    Right,
    None
}
