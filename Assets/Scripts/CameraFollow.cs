using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 targetOffset;
    public float cameraSpeed;
    public float cameraRotationSpeed;
    void Start() {
        targetOffset = target.position - transform.position;
    }
    void FixedUpdate() {
        // Dont need to look at anything if the car is stopped.
        if (target.GetComponent<Rigidbody>().velocity.magnitude < 0.1f) {
            // Dont need to look at anything if the car is stopped.
        } else{
            // Update the offset to be behind the car
            targetOffset = new Vector3(target.GetComponent<Rigidbody>().velocity.normalized.x * 25, -7, target.GetComponent<Rigidbody>().velocity.normalized.z * 17);
            // Move the camera to the new position
            transform.position = Vector3.Lerp(transform.position, target.position - targetOffset, cameraSpeed);
            // Make the camera look at a point in front of the car's velocity
            Vector3 lookAtPoint = target.position + target.GetComponent<Rigidbody>().velocity.normalized * target.GetComponent<Rigidbody>().velocity.magnitude * 0.2f;
            // This just works, thank you Copilot!
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtPoint - transform.position), cameraRotationSpeed * Mathf.Log(target.GetComponent<Rigidbody>().velocity.magnitude));
        }
    }
}
