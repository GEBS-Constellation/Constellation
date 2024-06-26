using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public float turnSpeed;
    public float turnForce;
    public float driftTurnSpeed;
    public bool isTrulyDrifting;
    public bool isReturningNormal;

    // This might be below the ground idk
    public float centerOfMassY = -0.2f;
    public Vector3 endOfDriftVel = new Vector3(0, 0, 0);
    public float driftReturnSpeed = 0.1f;
    public float karDistanceFromGround = 0.5f;
    public bool isGrounded;
    public Vector3 startPos;
    public Quaternion startRot;

    void Start()
    {
        // Initialise variables
        isTrulyDrifting = false;
        isReturningNormal = false;

        // Lower the center of mass to make the car more stable
        rb.centerOfMass = new Vector3(0, centerOfMassY, 0);

        // Set the start position
        startPos = rb.transform.position;
        startRot = rb.transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check if the car is on the ground
        if (Physics.Raycast(transform.position, -transform.up, karDistanceFromGround)) {
            // The car is on the ground, allow acceleration and drifting
            isGrounded = true;
        } else {
            // The car is not on the ground, don't allow acceleration and drifting
            isGrounded = false;
        }
        // Controls
        if (Input.GetKey("w") && isGrounded) {
            rb.AddForce(transform.forward * force);
        }
        if (Input.GetKey("s") && isGrounded) {
            rb.AddForce(-transform.forward * force);
        }
        int turn = 0;
        if (Input.GetKey("a")) {
            turn -= 1;
        }
        if (Input.GetKey("d")) {
            turn += 1;
        }

        // Turning of different types
        if (Input.GetKey(KeyCode.Space) && isGrounded) {
            // Drifting
            transform.Rotate(0, turn*driftTurnSpeed, 0);
            isTrulyDrifting = true;
            // It's not actually returning to normal driving yet, but it will when isTrulyDrifting is set to false
            isReturningNormal = true;
            endOfDriftVel = rb.velocity;
        
        } else {
            // Regular driving
            isTrulyDrifting = false;
            
            // Turn the car according to the turn variable
            float vel = rb.velocity.magnitude*0.15f;
            transform.Rotate(0, turn*(vel/(1+Mathf.Pow(vel-1, 2)))*turnSpeed, 0);

            // Rotate the velocity vector to the car's local space
            // This might desync the car's velocity from the car's rotation IDK

            // If not grounded, only allow rotation in the x and z axes
            if (!isGrounded) {
                rb.velocity = Quaternion.AngleAxis(turn*(vel/(1+Mathf.Pow(vel-1, 2)))*turnSpeed, new Vector3(0,1,0)) * rb.velocity;
            } else {
                rb.velocity = Quaternion.AngleAxis(turn*(vel/(1+Mathf.Pow(vel-1, 2)))*turnSpeed, transform.up) * rb.velocity;
            }
        }

        if (isReturningNormal && !isTrulyDrifting) {
            // Just stopped drifting, return car to normal driving
            Vector3 preVel = rb.velocity;

            // Use the movetowards function to smoothly return to normal driving
            rb.velocity = (Vector3.MoveTowards(rb.velocity.normalized, transform.forward, driftReturnSpeed))*rb.velocity.magnitude;

            if (transform.forward == rb.velocity.normalized) {
                // Kar fully returned to normal driving
                isReturningNormal = false;
            }
        }

        // If the car is on a slope, make it stick to the slope a little bit
        if (isGrounded) {
            // Calculate the angle between the car's up direction and the world up direction
            float angle = Vector3.Angle(transform.up, Vector3.up) - 90f;

            // Apply a downward force based on the angle to stick to the slope
            float downwardForce = -10 * Mathf.Atan(angle);
            rb.AddForce(transform.up * downwardForce);

            // Cancel out gravity to prevent excessive downward force
            float upwardForce = 10 * Mathf.Atan(angle);
            rb.AddForce(Vector3.up * upwardForce);
        }
            
        // Reset car position
        if (Input.GetKeyUp("r")) {
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);
            rb.transform.position = startPos;
            rb.transform.rotation = startRot;
        }

        
    }
}
