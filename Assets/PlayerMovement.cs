using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerHealthSystem))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float moveSpeedHorizontal = 10f,  tiltSpeed = 22.5f, upwardSpeed = 5f;
    [SerializeField] private float horizontalSmoothFactorTime = 0.1f;

    [SerializeField] private float maxTilt = 90f;
    
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private Transform thrust;
    private DefaultInputActions inputActions;

    private float positionX;
    private float currentVelocityX;
    private float currentTilt;

    private Vector3 moveDirection;

    private PlayerHealthSystem playerHealthSystem;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        positionX = transform.position.x;
        playerHealthSystem = GetComponent<PlayerHealthSystem>();
    }
    
    public void SetSpeed(float speed)
    {
        upwardSpeed = speed;
    }

    public float GetSpeed()
    {
        return upwardSpeed;
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
       
        if (Mathf.Abs(currentTilt) >= maxTilt)
        {
            AllignPlayer();
            playerHealthSystem.DamageIgnoreGodMod(25f);
            Debug.Log("Rocket critically tilted!");
        }

        currentTilt = Mathf.Clamp(currentTilt, -maxTilt, maxTilt);
        
        positionX += horizontalInput * moveSpeedHorizontal * Time.deltaTime;
        
        positionX = Mathf.Clamp(positionX, -10f, 10f);

        
    }

    private void AllignPlayer()
    {
        currentTilt = 0;
    }

    private void FixedUpdate()
    {
        float smoothX_Value = Mathf.SmoothDamp(transform.position.x, positionX, ref currentVelocityX,
            horizontalSmoothFactorTime);
        
        float verticalMovement = upwardSpeed * Time.fixedDeltaTime;
        
        float horizontalMovement = Mathf.Sin(Mathf.Deg2Rad * currentTilt) * moveSpeedHorizontal * Time.fixedDeltaTime;
        
      
        Vector3 movement = new Vector3(horizontalMovement, verticalMovement, 0);
        rigidbody.MovePosition(transform.position + movement);
        
        Quaternion targetRotation = Quaternion.Euler(0, 0, -currentTilt);
        rigidbody.MoveRotation(targetRotation);
    }
}
