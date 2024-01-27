using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float speed;
    void Start() {
        offset = transform.position - target.position;
    }
    void LateUpdate() {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, speed);
        transform.LookAt(target);
    }
}
