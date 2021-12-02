using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float rotationSpeed = 360.0f;
    [SerializeField] private float dashCd = .5f;
    [SerializeField] private GameObject aim;
    private bool canDash = true;
    private Camera _cam;
    public GameObject mesh;

    public bool mouseEnabled = true;

    private float _moveHorizontal;
    private float _moveVertical;
    private float _lookHorizontal;
    private float _lookVertical;
    private float _lastDashTime;
    private float _moveSpeed;

    private Vector3 _moveDirection;
    private Vector3 _velocity;

    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }


    private void Start()
    {
        _cam = Camera.main;
        _lastDashTime = Time.time;
        player = GetComponent<Player>();
        _moveSpeed = player.entityStats.speed;
        // anim = mesh.GetComponent<Animator>();
    }

    private void Update()
    {
        _moveDirection = Vector3.forward * _moveVertical + Vector3.right * _moveHorizontal;
        _velocity = _moveDirection * _moveSpeed;

        if (_moveDirection.magnitude < 0.1f)
        {
            // State = idle
            _velocity = Vector2.zero;
        }

        CheckForMouse();
        if (mouseEnabled)
        {
            RotateTowardMouseVector();
        }
        else
        {
            RotateTowardLookVector();
        }

    }

    private void FixedUpdate()
    {
        ApplyVelocity();
    }

    private void ApplyVelocity()
    {
        
        player.Rigidbody.AddForce(_velocity - player.Rigidbody.velocity,ForceMode.Acceleration);
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
            target.y = transform.position.y;
            var quat = Quaternion.LookRotation(target - transform.position, Vector3.up);
            player.Rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation,quat, rotationSpeed * Time.deltaTime));
            // mesh.transform.LookAt(target);
            if (aim)
                aim.transform.position = hitInfo.point;

        }
    }

    private void RotateTowardLookVector()
    {
        Vector3 lookDirection = Vector3.forward * _lookVertical + Vector3.right * _lookHorizontal + _moveVertical * Vector3.forward + _moveHorizontal * Vector3.right;

        if (lookDirection != Vector3.zero)
        {
                    transform.rotation = Quaternion.RotateTowards(mesh.transform.rotation,
                Quaternion.LookRotation(lookDirection, Vector3.up),
                rotationSpeed * Time.deltaTime);
        }
    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this._moveVertical = vertical;
        this._moveHorizontal = horizontal;
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
        if (player.DashAbility && Time.time > _lastDashTime + dashCd && canDash)
        {
            _lastDashTime = Time.time;
            StartCoroutine(player.AbilityCo(player.DashAbility.duration, player.DashAbility));
        } 
    }

}

