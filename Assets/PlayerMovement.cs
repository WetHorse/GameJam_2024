using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float  moveSpeedHorizontal = 10f, tiltSpeed = 22.5f, upwardSpeed = 5f;
    [SerializeField] private float horizontalSmoothFactorTime = 0.1f;
    [SerializeField] private float tiltRecoverySpeed = 20f;
    [SerializeField] private float maxTilt = 90f;
    
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private Transform thrust;
    private DefaultInputActions inputActions;

    private float positionX;
    private float currentVelocityX;
    private float currentTilt;
    

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        positionX = transform.position.x;

    }
    
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        positionX += horizontalInput * moveSpeedHorizontal * Time.deltaTime;
        positionX = Mathf.Clamp(positionX, -10f, 10f);
        currentTilt += horizontalInput * tiltSpeed * Time.deltaTime;

        if (horizontalInput < 0)
        {
            currentTilt = Mathf.MoveTowards(currentTilt, -maxTilt, tiltSpeed * Time.deltaTime);
        }
        else if (horizontalInput > 0) 
        {
            currentTilt = Mathf.MoveTowards(currentTilt, maxTilt, tiltSpeed * Time.deltaTime);
        }
        else
        {
            currentTilt = Mathf.MoveTowards(currentTilt, 0, tiltRecoverySpeed * Time.deltaTime);
        }
        
        if (Mathf.Abs(currentTilt) > 60f)
        {
            Debug.Log("Rocket critically tilted!");
        }

        currentTilt = Mathf.Clamp(currentTilt, -maxTilt, maxTilt);
        
        positionX += horizontalInput * moveSpeedHorizontal * Time.deltaTime;
        
        positionX = Mathf.Clamp(positionX, -10f, 10f);
    }

    private void FixedUpdate()
    {
        float smoothX_Value = Mathf.SmoothDamp(transform.position.x, positionX, ref currentVelocityX,
            horizontalSmoothFactorTime);
        
        rigidbody.MovePosition(new Vector3(smoothX_Value, transform.position.y + upwardSpeed * Time.fixedDeltaTime, transform.position.z));
        
        Quaternion targetRotation = Quaternion.Euler(0, 0, -currentTilt);
        rigidbody.MoveRotation(targetRotation);
    }
}
