using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[Serializable] public class MoveInputEvent : UnityEvent<float, float> {}
[Serializable] public class LookInputEvent : UnityEvent<float, float> {}
[Serializable] public class dashInputEvent : UnityEvent {}
public class InputController : MonoBehaviour
{
    private Controls controls;
    public MoveInputEvent moveInputEvent;
    public LookInputEvent lookInputEvent;
    public dashInputEvent dashInputEvent;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Move.performed += OnMovePerformed;
        controls.Gameplay.Move.canceled += OnMovePerformed;
        controls.Gameplay.Look.performed += OnLookPerformed;
        controls.Gameplay.Look.canceled += OnLookPerformed;
        controls.Gameplay.Dash.performed += _ => OnDashPerformed(); // we don't need the context here
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        Vector2 moveInput = ctx.ReadValue<Vector2>();
        moveInputEvent.Invoke(moveInput.x, moveInput.y);
        // Debug.Log($"Move Input: {moveInput.ToString()}");
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 lookInput = ctx.ReadValue<Vector2>();
        lookInputEvent.Invoke(lookInput.x, lookInput.y);
    }

    private void OnDashPerformed()
    {
        dashInputEvent.Invoke();
    }
    
}
