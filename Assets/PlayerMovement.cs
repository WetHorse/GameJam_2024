using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0,100)] private float moveSpeedVertical, moveSpeedHorizontal, rotationSpeed;
    [SerializeField] private Rigidbody rigidbody;

    private DefaultInputActions inputActions;
    

    void Start()
    {
        inputActions = new DefaultInputActions();
        inputActions.Player.Enable();
        inputActions.Player.MoveLeft.performed += OnMovedLeft;
        inputActions.Player.MoveRight.performed += OnMovedRight;
    }

    private void OnMovedLeft(InputAction.CallbackContext obj)
    {
        rigidbody.AddForce(transform.right * moveSpeedHorizontal * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnMovedRight(InputAction.CallbackContext obj)
    {
        rigidbody.AddForce(-transform.right * moveSpeedHorizontal * Time.deltaTime, ForceMode.Impulse);
    }

    void Update()
    {
        //if (!Input.GetKey(KeyCode.Space)) return;
        rigidbody.AddForce(Vector3.up * moveSpeedVertical * Time.deltaTime,ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, rigidbody.velocity.y * rotationSpeed * Time.deltaTime);
    }
}
