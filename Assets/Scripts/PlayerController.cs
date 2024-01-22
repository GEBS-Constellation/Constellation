/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Reference to the character controller component
    public CharacterController controller;

    // Movement speed
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get horizontal and vertical input
        bool controlLeft = Input.GetKey("a");
        bool controlRight = Input.GetKey("d");

        // Translate input to coefficients
        float x = 0f;
        
        if (controlLeft)
        {
            x -= speed;  
        }
        if (controlRight)
        {
            x += speed;
        }

        // Create a vector based on the input
        Vector2 move = transform.right * x;

        // Move the character controller
        controller.Move(move * Time.deltaTime);
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float playerRotationSpeed = 0.4f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float frictionConstant = 0.001f;
    [SerializeField]
    private float airResistanceConstant = 0.01f;


    private CharacterController controller;
    private Vector3 playerVelocity;
    private PlayerInput playerInput;
    private bool groundedPlayer;
    private float playerForwardSpeed;

    private InputAction moveAction;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        playerVelocity = new Vector3(0,0,0);
        playerForwardSpeed = 0f;
    }
    

    void Update()
    {
        // The vehicle must not fall through the ground
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        
        // Speed and rotation based movement //

        // This will not work on a controller because the movement will be "boolean". lmao
        if (input.y > 0)
        {
            playerForwardSpeed += playerSpeed;
        } else if (input.y < 0)
        {
            playerForwardSpeed -= playerSpeed;
        }

        if (input.x > 0)
        {
            float r = 0.4f;
            float vx = Mathf.Pow(playerForwardSpeed, 2) / r * Time.deltaTime;
            //gameObject.transform.Rotate(0, Mathf.Abs(playerForwardSpeed) > 0 ? Mathf.Atan(vx / playerForwardSpeed) : 0, 0);
            gameObject.transform.Rotate(0, playerForwardSpeed * Mathf.Atan(1 / playerForwardSpeed), 0);
        } else if (input.x < 0)
        {
            float r = 0.4f;
            float vx = Mathf.Pow(playerForwardSpeed, 2) / r * Time.deltaTime;
            //gameObject.transform.Rotate(0, Mathf.Abs(playerForwardSpeed) > 0 ? -Mathf.Atan(vx / playerForwardSpeed) : 0, 0);
            gameObject.transform.Rotate(0, -playerForwardSpeed * Mathf.Atan(1 / playerForwardSpeed), 0);
        }

        // Idea: Newton 2 law, car mass (acc/deceleration)
        


        /*
        // Vector-based movement //


        // This will not work on a controller because the movement will be "boolean". lmao
        Vector3 move = new Vector3(0,0,0);
        if (input.x > 0)
        {
            move += transform.right * playerRotationSpeed * Mathf.Atan(playerVelocity.magnitude);
        } else if (input.x < 0)
        {
            move -= transform.right * playerRotationSpeed * Mathf.Atan(playerVelocity.magnitude);
        }

        if (input.y > 0)
        {
            move += transform.forward * playerSpeed;
        } else if (input.x < 0)
        {
            move -= transform.forward * playerSpeed;
        }


        playerVelocity += move;
        
        // Friction
        Vector3 friction = -playerVelocity * frictionConstant;
        Vector3 airResistance = new Vector3(-(float)Math.Abs(playerVelocity.x) * playerVelocity.x * airResistanceConstant, 
                                            -(float)Math.Abs(playerVelocity.y) * playerVelocity.y * airResistanceConstant, 
                                            -(float)Math.Abs(playerVelocity.z) * playerVelocity.z * airResistanceConstant);

        playerVelocity += friction;
        playerVelocity += airResistance;

                // If the player isn't moving in both the x and z direction
        if (playerVelocity.x == 0 && playerVelocity.z == 0)
        {
            // Do nothing
        } else
        {
            // Update the player heading
            gameObject.transform.forward = new Vector3(playerVelocity.x, 0, playerVelocity.z);
        }
        */

        playerVelocity = transform.forward * playerForwardSpeed;

        // Friction
        playerVelocity -= transform.forward * frictionConstant * playerForwardSpeed;
        playerVelocity -= transform.forward * airResistanceConstant * playerForwardSpeed * Math.Abs(playerForwardSpeed);

        // Y-axis movement will break this probably
        playerForwardSpeed = playerVelocity.magnitude;

        // This screws everything up 
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);



    }
}
