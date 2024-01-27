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
            transform.Rotate(0, turn, 0);
        } else {
            transform.Rotate(0, turn*Mathf.Atan(rb.velocity.magnitude)*turnSpeed, 0);
            rb.velocity = transform.forward * rb.velocity.magnitude;
        }
        
    }
}
