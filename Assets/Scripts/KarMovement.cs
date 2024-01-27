using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public float turnSpeed;

    // Update is called once per frame
    void Update()
    {
        // Controls
        if (Input.GetKey("w")) {
            rb.AddForce(transform.forward * force);
        }
        int turn = 0;
        if (Input.GetKey("a")) {
            turn -= 1;
        }
        if (Input.GetKey("d")) {
            turn += 1;
        }

        //Turning
        if (Input.GetKey(KeyCode.Space)) {
            transform.Rotate(0, turn*Time.deltaTime*300, 0);
        } else {
            transform.Rotate(0, turn*Mathf.Atan(rb.velocity.magnitude)*turnSpeed, 0);
            float y_vel = rb.velocity.y;
            rb.velocity -= new Vector3(0, rb.velocity.y, 0);
            Vector3 forward = transform.forward;
            forward.y = 0;
            forward = forward.normalized;
            rb.velocity = forward * rb.velocity.magnitude;
            rb.velocity += new Vector3(0, y_vel, 0);
        }
        
    }
}
