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
    public bool isDrifting;

    // This might be below the ground idk
    public float centerOfMassY = -0.2f;

    void Start()
    {
        isDrifting = false;

        // Lower the center of mass to make the car more stable
        rb.centerOfMass = new Vector3(0, centerOfMassY, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Controls
        if (Input.GetKey("w")) {
            rb.AddForce(transform.forward * force);
        }
        if (Input.GetKey("s")) {
            rb.AddForce(-transform.forward * force);
        }
        int turn = 0;
        if (Input.GetKey("a")) {
            turn -= 1;
            if (!isDrifting){
                //rb.AddForce(-transform.right * turnForce*Mathf.Atan(Vector3.Dot(rb.velocity, transform.forward))*Time.deltaTime*200);
                rb.AddForce(-(rb.velocity - (Vector3.Dot(rb.velocity, transform.forward)*transform.forward)));
            }
        }
        if (Input.GetKey("d")) {
            turn += 1;
            if (!isDrifting){
                //rb.AddForce(transform.right * turnForce*Mathf.Atan(Vector3.Dot(rb.velocity, transform.forward))*Time.deltaTime*200);
                rb.AddForce(-(rb.velocity - (Vector3.Dot(rb.velocity, transform.forward)*transform.forward)));
            }
        }

        //Turning
        if (Input.GetKey(KeyCode.Space)) {
            // Drifting
            transform.Rotate(0, turn*driftTurnSpeed, 0);
            isDrifting = true;
        } else {
            // Regular driving
            isDrifting = false;
/*             transform.Rotate(0, turn*Mathf.Atan(rb.velocity.magnitude)*turnSpeed, 0);
            float y_vel = rb.velocity.y;
            rb.velocity -= new Vector3(0, rb.velocity.y, 0);
            Vector3 forward = transform.forward;
            forward.y = 0;
            forward = forward.normalized;
            rb.velocity = forward * rb.velocity.magnitude;
            rb.velocity += new Vector3(0, y_vel, 0); */
            
            // transform.Rotate(0, turn*Mathf.Atan(rb.velocity.magnitude*0.5f)*turnSpeed*Time.deltaTime*100, 0);
            float vel = rb.velocity.magnitude*0.15f;
            transform.Rotate(0, turn*(vel/(1+Mathf.Pow(vel-1, 2)))*turnSpeed, 0);

            // Rotate the velocity vector to the car's local space
            // This might desync the car's velocity from the car's rotation IDK
            rb.velocity = Quaternion.AngleAxis((turn*(vel/(1+Mathf.Pow(vel-1, 2)))*turnSpeed), transform.up) * rb.velocity;


        }

        // Reset car position
        if (Input.GetKeyUp("r")) {
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);
            rb.transform.position = new Vector3(-24.9f, 1.23f, -8.1f);
            rb.transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        
    }
}
