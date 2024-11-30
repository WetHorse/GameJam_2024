using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float  tiltSpeed = 22.5f, moveSpeed = 10f;
    [SerializeField] private float horizontalSmoothFactorTime = 0.1f;
    [SerializeField] private float tiltRecoverySpeed = 20f;
    [SerializeField] private float maxTilt = 90f;
    
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private Transform thrust;
    private DefaultInputActions inputActions;

    private float positionX;
    private float currentVelocityX;
    private float currentTilt;

    private Vector3 moveDirection;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        positionX = transform.position.x;

    }
    
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        
        if (horizontalInput < 0)
        {
            currentTilt = Mathf.MoveTowards(currentTilt, -maxTilt, tiltSpeed * Time.deltaTime);
        }
        else if (horizontalInput > 0) 
        {
            currentTilt = Mathf.MoveTowards(currentTilt, maxTilt, tiltSpeed * Time.deltaTime);
        }
       
        if (Mathf.Abs(currentTilt) > 60f)
        {
            Debug.Log("Rocket critically tilted!");
        }

        currentTilt = Mathf.Clamp(currentTilt, -maxTilt, maxTilt);
        
        positionX += horizontalInput * moveSpeed * Time.deltaTime;
        
        positionX = Mathf.Clamp(positionX, -10f, 10f);

        moveDirection = transform.forward;
    }

    private void FixedUpdate()
    {
        float smoothX_Value = Mathf.SmoothDamp(transform.position.x, positionX, ref currentVelocityX,
            horizontalSmoothFactorTime);

        Vector3 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
        
        rigidbody.MovePosition(transform.position + movement);
        
        rigidbody.MovePosition(new Vector3(smoothX_Value, transform.position.y, transform.position.z));
        
        Quaternion targetRotation = Quaternion.Euler(0, 0, -currentTilt);
        rigidbody.MoveRotation(targetRotation);
    }
}
