using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "InputReader", menuName = "InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action PlaceObjectEvent;

    public Vector2 MovementValue { get; private set; }
    public float HoverValue { get; private set; }
    public Vector2 MousePositionValue { get; private set; }
    public float RotateValue { get; private set; }

    private Controls _controls;


    private void OnEnable()
    {
        _controls ??= new Controls();
        _controls.Player.SetCallbacks(this);
        _controls.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnHover(InputAction.CallbackContext context)
    {
        HoverValue = context.ReadValue<float>();
    }

    public void OnPlaceObject(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        PlaceObjectEvent?.Invoke();
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        MousePositionValue = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        RotateValue = context.ReadValue<float>();
    }
}