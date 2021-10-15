using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private float rotationSpeed = 360.0f;
    [SerializeField] private float dashSpeed = 100.0f;
    [SerializeField] private float dashTime = .1f;
    [SerializeField] private float dashCD = .5f;
    
    private Camera _cam;
    private Animator _anim;

    [SerializeField] private GameObject mesh;

    public bool mouseEnabled = true;

    private float _moveHorizontal;
    private float _moveVertical;
    private float _lookHorizontal;
    private float _lookVertical;
    private float _lastDashTime;

    private Vector3 _moveDirection;

    private Rigidbody _rigidbody;
    private WeaponController _weaponController;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _weaponController = GetComponent<WeaponController>();
        _cam = Camera.main;
        _lastDashTime = Time.time;
        _anim = mesh.GetComponent<Animator>();
        
    }

    private void Update()
    {
        _moveDirection = Vector3.forward * _moveVertical + Vector3.right * _moveHorizontal;


        CheckForMouse();
        if (mouseEnabled)
        {
            RotateTowardMouseVector();
        }
        else
        {
            RotateTowardLookVector();
        }

        Move(_moveDirection * moveSpeed);
    }


    private void Move(Vector3 velocity)
    {
        //transform.position += velocity;
        _rigidbody.AddForce(velocity, ForceMode.Acceleration);
    }

    private void CheckForMouse()
    {
        Mouse mouse = InputSystem.GetDevice<Mouse>();
        if (mouse.leftButton.isPressed)
        {
            mouseEnabled = true;
        }
    }

    private void RotateTowardMouseVector()
    {
        Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());


        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            Vector3 target = hitInfo.point;
            target.y = mesh.transform.position.y;
            mesh.transform.LookAt(target);

        }
    }

    private void RotateTowardLookVector()
    {
        Vector3 lookDirection = Vector3.forward * _lookVertical + Vector3.right * _lookHorizontal + _moveVertical * Vector3.forward + _moveHorizontal * Vector3.right;

        if (lookDirection != Vector3.zero)
        {
            mesh.transform.rotation = Quaternion.RotateTowards(mesh.transform.rotation,
                Quaternion.LookRotation(lookDirection, Vector3.up),
                rotationSpeed * Time.deltaTime);

        }
    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this._moveVertical = vertical;
        this._moveHorizontal = horizontal;
        //Debug.Log($"Player Controller: Move Input: ({vertical.ToString()}, {horizontal.ToString()})");
    }

    public void OnLookInput(float horizontal, float vertical)
    {
        mouseEnabled = false;
        this._lookVertical = vertical;
        this._lookHorizontal = horizontal;
    }

    public void OnDashInput()
    {

        // Check for dash cooldown
        if (Time.time > _lastDashTime + dashCD) StartCoroutine(Dash());
    }

    public void OnMeleeInput()
    {
        _anim.Play("MeleeAttack");
    }

    public void OnMeleeChargeInput()
    {
    }

    public void OnRangedInput()
    {
        _weaponController.Cast();
    }
    
    public void OnRangedChargeInput()
    {
    }

    private IEnumerator Dash()
    {
        float startTime = Time.time;
        _lastDashTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            // Could use the moveDirection or playerForward
            Move(_moveDirection * dashSpeed);
            //Move(player.transform.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }

    }



}

