using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float moveSpeedVertical, moveSpeedHorizontal, rotationSpeed;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform thrust;
    private DefaultInputActions inputActions;

    void Start()
    {
        inputActions = new DefaultInputActions();
        inputActions.Player.Enable();
        inputActions.Player.MoveLeft.performed += OnMovedLeft;
       // inputActions.Player.MoveLeft.canceled += OnMovedLeft;
        inputActions.Player.MoveRight.performed += OnMovedRight;
        //inputActions.Player.MoveRight.canceled += OnMovedRight;
    }

    private void OnMovedLeft(InputAction.CallbackContext obj)
    {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0);
        rigidbody.AddForceAtPosition(-transform.right * rotationSpeed, thrust.position, ForceMode.Impulse);
    }

    private void OnMovedRight(InputAction.CallbackContext obj)
    {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0);
        rigidbody.AddForceAtPosition(transform.right * rotationSpeed, thrust.position, ForceMode.Impulse);
    }


    void Update()
    {
     
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeedHorizontal,moveSpeedVertical, rigidbody.velocity.z);
    }
}
