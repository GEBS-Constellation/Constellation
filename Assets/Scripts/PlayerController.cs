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
    private float gravityValue = -9.81f;
    [SerializeField]
    private float frictionConstant = 0.001f;
    [SerializeField]
    private float airResistanceConstant = 0.01f;


    private CharacterController controller;
    private Vector3 playerVelocity;
    private PlayerInput playerInput;
    private bool groundedPlayer;

    private InputAction moveAction;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        playerVelocity = new Vector3(0,0,0);
    }
    

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x * playerSpeed, 0, input.y * playerSpeed);
        playerVelocity += move;
        
        // Friction
        Vector3 friction = -playerVelocity * frictionConstant;
        Vector3 airResistance = new Vector3(-(float)Math.Pow(playerVelocity.x, 2f) * airResistanceConstant, 
                                            -(float)Math.Pow(playerVelocity.y, 2f) * airResistanceConstant, 
                                            -(float)Math.Pow(playerVelocity.z, 2f) * airResistanceConstant);

        playerVelocity += friction;
        
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (playerVelocity.x != 0 && playerVelocity.y != 0)
        {
            gameObject.transform.forward = new Vector3(playerVelocity.x, 0, playerVelocity.y);
        }

    }
}
