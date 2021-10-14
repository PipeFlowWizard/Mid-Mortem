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
    private Camera cam;

    [SerializeField] private GameObject player;

    public bool mouseEnabled = true;

    private float moveHorizontal;
    private float moveVertical;
    private float lookHorizontal;
    private float lookVertical;
    private float lastDashTime;

    private Vector3 moveDirection;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        cam = Camera.main;
        lastDashTime = Time.time;
    }

    private void Update()
    {
        moveDirection = Vector3.forward * moveVertical + Vector3.right * moveHorizontal;


        CheckForMouse();
        if (mouseEnabled)
        {
            RotateTowardMouseVector();
        }
        else
        {
            RotateTowardLookVector();
        }

        Move(moveDirection * moveSpeed);
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
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());


        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            Vector3 target = hitInfo.point;
            target.y = player.transform.position.y;
            player.transform.LookAt(target);

        }
    }

    private void RotateTowardLookVector()
    {
        Vector3 lookDirection = Vector3.forward * lookVertical + Vector3.right * lookHorizontal + moveVertical * Vector3.forward + moveHorizontal * Vector3.right;

        if (lookDirection != Vector3.zero)
        {
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation,
                Quaternion.LookRotation(lookDirection, Vector3.up),
                rotationSpeed * Time.deltaTime);

        }
    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this.moveVertical = vertical;
        this.moveHorizontal = horizontal;
        //Debug.Log($"Player Controller: Move Input: ({vertical.ToString()}, {horizontal.ToString()})");
    }

    public void OnLookInput(float horizontal, float vertical)
    {
        mouseEnabled = false;
        this.lookVertical = vertical;
        this.lookHorizontal = horizontal;
    }

    public void OnDashInput()
    {

        // Check for dash cooldown
        if (Time.time > lastDashTime + dashCD) StartCoroutine(Dash());
    }

    public void OnMeleeInput()
    {
    }

    public void OnMeleeHoldInput()
    {
    }

    public void OnRangedInput()
    {
    }
    
    public void OnRangedHoldInput()
    {
    }

    private IEnumerator Dash()
    {
        float startTime = Time.time;
        lastDashTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            // Could use the moveDirection or playerForward
            Move(moveDirection * dashSpeed);
            //Move(player.transform.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }

    }



}

