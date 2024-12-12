using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

public enum ControllerMode
{
    UI,
    Battle
}

[CreateAssetMenu(fileName = "InputReader", menuName = "InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action PlaceObjectEvent;
    public event Action TogglePauseEvent;

    public Vector2 MovementValue { get; private set; }
    public float HoverValue { get; private set; }
    public Vector2 MousePositionValue { get; private set; }
    public float RotateValue { get; private set; }

    private Controls _controls;
    private ControllerMode _controllerMode;

    private void OnEnable()
    {
        _controls ??= new Controls();
        _controls.Player.SetCallbacks(this);
    }

    public void SetControllerMode(ControllerMode mode)
    {
        _controllerMode = mode;

        switch (_controllerMode)
        {
            case ControllerMode.UI:
                _controls.Player.Disable();
                _controls.UI.Enable();
                break;
            case ControllerMode.Battle:
                _controls.UI.Disable();
                _controls.Player.Enable();
                break;
        }
    }

    #region Actions

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

    public void OnTogglePause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        TogglePauseEvent?.Invoke();
    }

    #endregion
}