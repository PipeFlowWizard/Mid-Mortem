using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;

[Serializable] public class MoveInputEvent : UnityEvent<float, float> {}
[Serializable] public class LookInputEvent : UnityEvent<float, float> {}
[Serializable] public class DashInputEvent : UnityEvent {}
[Serializable] public class MeleeInputEvent : UnityEvent {}
[Serializable] public class MeleeChargeInputEvent : UnityEvent {}
[Serializable] public class RangedInputEvent : UnityEvent {}
[Serializable] public class RangedChargeInputEvent : UnityEvent {}
public class InputController : MonoBehaviour
{
    private Controls _controls;
    public MoveInputEvent moveInputEvent;
    public LookInputEvent lookInputEvent;
    public DashInputEvent dashInputEvent;
    public MeleeInputEvent meleeInputEvent;
    public MeleeChargeInputEvent meleeChargeInputEvent;
    public RangedInputEvent rangedInputEvent;
    public RangedChargeInputEvent rangedChargeInputEvent;

    private void Awake()
    {
        _controls = new Controls();
    }

    private void OnEnable()
    {
        // Gameplay (movement)
        _controls.Gameplay.Enable();
        _controls.Gameplay.Move.performed += OnMovePerformed;
        _controls.Gameplay.Move.canceled += OnMovePerformed;
        _controls.Gameplay.Look.performed += OnLookPerformed;
        _controls.Gameplay.Look.canceled += OnLookPerformed;
        _controls.Gameplay.Dash.performed += _ => OnDashPerformed(); // we don't need the context here

        // Action
        _controls.Action.Enable();
        _controls.Action.MeleeAttack.performed += OnMeleePerformed;
        _controls.Action.RangedAttack.performed += OnRangedPerformed;

    }


    private void OnDisable()
    {
        _controls.Gameplay.Disable();
        _controls.Action.Disable();
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

    private void OnMeleePerformed(InputAction.CallbackContext ctx)
    {
        switch (ctx.interaction)
        {
            case TapInteraction _:
                meleeInputEvent.Invoke();
                Debug.Log("Melee attack");
                break;
            case SlowTapInteraction _:
                meleeChargeInputEvent.Invoke();
                Debug.Log("Melee charged attack");
                break;
        }
    }
    
    private void OnRangedPerformed(InputAction.CallbackContext ctx)
    {
        switch (ctx.interaction)
        {
            case TapInteraction _:
                rangedInputEvent.Invoke();
                Debug.Log("Ranged attack");
                break;
            case SlowTapInteraction _:
                rangedChargeInputEvent.Invoke();
                Debug.Log("Ranged charged attack");
                break;
        }
    }



}
