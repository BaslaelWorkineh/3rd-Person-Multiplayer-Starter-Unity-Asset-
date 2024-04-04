using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public float gravity = 30f;
    public float walkSpeed = 4f;
    public float sprintSpeed = 14f;
    public float maxVelocityChange = 10f;
    [Space]
    public float airControl = 0.5f;

    [Space]
    public float jumpHeight = 13f;

    private Vector2 input;
    private Rigidbody rb;

    private bool sprinting;
    private bool jumping;

    private bool grounded = false;

    public FixedJoystick joystick;
    public AudioClip jumpSound; // Variable to store the shooting sound effect
    public AudioSource audioSource; // Reference to the AudioSource component
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(joystick.Horizontal, joystick.Vertical);
        input.Normalize();

        // sprinting = Input.GetButton("Sprint");
        // jumping = Input.GetButton("Jump");
        if(CrossPlatformInputManager.GetButton("Sprint"))
        {
            sprinting = true;
        }

    }

    public void jumpButton()
    {
        if(grounded)
        {
            jumping = true;
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        }
            
    }

    private void OnTriggerStay(Collider other)
    {
        grounded = true;
    }

    void FixedUpdate()
    {
        if(grounded)
        {
           if (jumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, CalculateJumpVerticalSpeed(), rb.velocity.z);
                jumping = false;
            }
            else if(input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(sprinting? sprintSpeed : walkSpeed), ForceMode.VelocityChange);
                sprinting = false;
            }
            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        }
        else
        {
            rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));
            if(input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(sprinting? sprintSpeed * airControl: walkSpeed * airControl), ForceMode.VelocityChange);
            }
            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        }

       grounded = false;
    }

    float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    Vector3 CalculateMovement(float _speed)
    {
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);

        targetVelocity *= _speed;

        Vector3 velocity = rb.velocity;

        if(input.magnitude > 0.5f)
        {
            Vector3 velocityChange = targetVelocity - velocity;

            velocityChange.x = Mathf.Clamp(velocityChange.x,-maxVelocityChange, maxVelocityChange);
            velocityChange.x = Mathf.Clamp(velocityChange.x,-maxVelocityChange, maxVelocityChange);

            velocityChange.y = 0;

            return(velocityChange);
        }
        else
        {
            return new Vector3();
        }
    }
}
