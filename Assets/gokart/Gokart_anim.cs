using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Gokart_anim : MonoBehaviour
{
    public Transform player;
    public Transform[] wheels = new Transform[] {};
    public Transform body;
    public Transform steering_wheel;
    public float turning_angle;
    public float turning_speed;
    public float wheel_speed;
    public float vibration_speed;
    public float vibration_strength;
    public float lean_strength;
    public float lean_speed;
    public float turn;
    public float gas;
    public float vel;
    //Wheels turn
    //Steering wheel turn
    //Vibration
    //Lean
    //Exhaust fire
    //Tire smoke
    void Start() {
        
    }
    void Update() {
        // Input
        turn = 0;
        gas = 0;
        if (Input.GetKey("a")) {
            turn -= 1;
        }
        if (Input.GetKey("d")) {
            turn += 1;
        }
        if (Input.GetKey("w")) {
            gas = 0.1f;
            vel += gas;
        }
        vel -= 0.002f*vel;
        print(vel);

        // Wheels turning animation
        for (int i = 0; i < wheels.Length; i++) {
            if (i < 2) {
                Vector3 wheelRot = wheels[i].localEulerAngles;
                wheels[i].localRotation = Quaternion.Lerp(wheels[i].localRotation, Quaternion.Euler(new Vector3(wheelRot.x, turn*turning_angle, wheelRot.z)), turning_speed);
            }
            float rot = vel*wheel_speed;
            /*
            if (i > 1 && gas > 0 && vel < 25) {
                rot = 3;
            }
            */
            wheels[i].Rotate(Vector3.forward,-rot,Space.Self);
        }

        // Steering wheel rotation
        Vector3 steeringWheelRot = steering_wheel.localEulerAngles;
        steering_wheel.localRotation = Quaternion.Lerp(steering_wheel.localRotation, Quaternion.Euler(new Vector3(steeringWheelRot.x, steeringWheelRot.y, 90+turn*turning_angle)), turning_speed);

        // Car vibration
        float noisex = Mathf.PerlinNoise(Time.timeSinceLevelLoad * vibration_speed, 0)-0.5f;
        float noisey = Mathf.PerlinNoise(Time.timeSinceLevelLoad * vibration_speed, 10)-0.5f;
        float noisez = Mathf.PerlinNoise(Time.timeSinceLevelLoad * vibration_speed, 20)-0.5f;
        float strength = vibration_strength * Mathf.Lerp(1, 2, gas*10f);
        body.localPosition = new Vector3(noisex * strength, noisey * strength, noisez * strength);

        // Car lean
        body.localRotation = Quaternion.Lerp(body.localRotation, Quaternion.Euler(-turn*lean_strength, 0, 0), lean_speed);
    }
}
