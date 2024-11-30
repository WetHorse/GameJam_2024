using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float moveSpeedVertical =10f, moveSpeedHorizontal = 10f, rotationSpeed = 5f, upwardSpeed = 5f;
    [SerializeField] private float horizontalSmoothFactorTime = 0.1f;
    
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform thrust;
    private DefaultInputActions inputActions;

    private float positionX;
    private float currentVelocityX;
    

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        
        inputActions = new DefaultInputActions();
        inputActions.Player.Enable();
        positionX = transform.position.x;

    }

    // private void OnMovedLeft(InputAction.CallbackContext obj)
    // {
    //     rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0);
    //     rigidbody.AddForceAtPosition(-transform.right * rotationSpeed, thrust.position, ForceMode.Impulse);
    // }
    //
    // private void OnMovedRight(InputAction.CallbackContext obj)
    // {
    //     rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0);
    //     rigidbody.AddForceAtPosition(transform.right * rotationSpeed, thrust.position, ForceMode.Impulse);
    // }


    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        positionX += horizontalInput * moveSpeedHorizontal * Time.deltaTime;
        positionX = Mathf.Clamp(positionX, -10f, 10f);
    }

    private void FixedUpdate()
    {
        float smoothX_Value = Mathf.SmoothDamp(transform.position.x, positionX, ref currentVelocityX,
            horizontalSmoothFactorTime);
        // rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeedHorizontal,moveSpeedVertical, rigidbody.velocity.z);
        rigidbody.MovePosition(new Vector3(smoothX_Value, transform.position.y + upwardSpeed * Time.fixedDeltaTime, transform.position.z));
    }
}
