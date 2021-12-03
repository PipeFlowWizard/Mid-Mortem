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
[Serializable] public class Ability1InputEvent : UnityEvent {}
[Serializable] public class Ability2InputEvent : UnityEvent {}
[Serializable] public class MeleeInputEvent : UnityEvent {}
[Serializable] public class MeleeChargeInputEvent : UnityEvent {}
[Serializable] public class RangedInputEvent : UnityEvent {}
[Serializable] public class RangedChargeInputEvent : UnityEvent {}
[Serializable] public class PauseGameInputEvent : UnityEvent {}
[Serializable] public class HealthInputEvent : UnityEvent<float> {}
[Serializable] public class SoulQueueInputEvent : UnityEvent<int> { }

public class InputController : MonoBehaviour
{
    private Controls _controls;
    public MoveInputEvent moveInputEvent;
    public LookInputEvent lookInputEvent;
    public DashInputEvent dashInputEvent;
    public Ability1InputEvent ability1InputEvent;
    public Ability2InputEvent ability2InputEvent;
    public MeleeInputEvent meleeInputEvent;
    public MeleeChargeInputEvent meleeChargeInputEvent;
    public RangedInputEvent rangedInputEvent;
    public RangedChargeInputEvent rangedChargeInputEvent;
    public PauseGameInputEvent pauseGameInputEvent;
    public HealthInputEvent healthInputEvent;
    public SoulQueueInputEvent soulQueueInputEvent;

    private bool _isPaused = false;

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
        _controls.Action.Ability1.performed += _ => OnAbility1Performed(); 
        _controls.Action.Ability2.performed += _ => OnAbility2Performed(); 

        // UI
        _controls.UI.Enable();
        _controls.UI.PauseGame.performed += _ => OnPausePerformed();

        // Debug
        _controls.Debug.Enable();
        _controls.Debug.Health.performed += OnHealthPerformed;
        _controls.Debug.SoulQueue.performed += OnSoulQueuePerformed;

    }


    private void OnDisable()
    {
        _controls.Gameplay.Disable();
        _controls.Action.Disable();
        _controls.Debug.Disable();
        _controls.UI.Disable();
    }

    private void DisableForPause()
    {
        _isPaused = true;
        _controls.Gameplay.Disable();
        _controls.Action.Disable();
    }

    private void EnableForPause()
    {
        _isPaused = false;
        _controls.Gameplay.Enable();
        _controls.Action.Enable();
    }

    public void ToggleActionMap()
    {
        if (_isPaused)
        {
            EnableForPause();
        }
        else
        {
            DisableForPause();
        }
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

    private void OnAbility1Performed()
    {
        ability1InputEvent.Invoke();
    }
    private void OnAbility2Performed()
    {
        ability2InputEvent.Invoke();
    }
    
    private void OnMeleePerformed(InputAction.CallbackContext ctx)
    {
        switch (ctx.interaction)
        {
            case TapInteraction _:
                meleeInputEvent.Invoke();
                break;
            case SlowTapInteraction _:
                meleeChargeInputEvent.Invoke();
                break;
        }
    }
    
    private void OnRangedPerformed(InputAction.CallbackContext ctx)
    {
        switch (ctx.interaction)
        {
            case TapInteraction _:
                rangedInputEvent.Invoke();
                break;
            case SlowTapInteraction _:
                rangedChargeInputEvent.Invoke();
                break;
        }
    }

    private void OnPausePerformed()
    {
        pauseGameInputEvent.Invoke();
    }

    private void OnHealthPerformed(InputAction.CallbackContext ctx)
    {
        healthInputEvent.Invoke(ctx.ReadValue<float>());
    }

    private void OnSoulQueuePerformed(InputAction.CallbackContext ctx)
    {
        soulQueueInputEvent.Invoke((int) ctx.ReadValue<float>());
    }


    
}
