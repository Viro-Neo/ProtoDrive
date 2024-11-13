using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private GameObject wheelFrontLeft;
    private GameObject wheelFrontRight;
    private GameObject wheelRearLeft;
    private GameObject wheelRearRight;

    private readonly float maxSteerAngle = 30;
    private readonly float motorForce = 50;

    // Start is called before the first frame update
    void Start()
    {
        wheelFrontLeft = GameObject.Find("FL");
        wheelFrontRight = GameObject.Find("FR");
        wheelRearLeft = GameObject.Find("RL");
        wheelRearRight = GameObject.Find("RR");
    }

    private void UpdateWheelPose(GameObject wheel, WheelCollider collider)
    {
        Vector3 pos = wheel.transform.position;
        Quaternion quat = wheel.transform.rotation;

        collider.GetWorldPose(out pos, out quat);

        wheel.transform.position = pos;
        wheel.transform.rotation = quat;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float steerAngle = maxSteerAngle * horizontalInput;
        float motorTorque = motorForce * verticalInput;

        wheelFrontLeft.GetComponent<WheelCollider>().steerAngle = steerAngle;
        wheelFrontRight.GetComponent<WheelCollider>().steerAngle = steerAngle;

        wheelFrontLeft.GetComponent<WheelCollider>().motorTorque = motorTorque;
        wheelFrontRight.GetComponent<WheelCollider>().motorTorque = motorTorque;
        wheelRearLeft.GetComponent<WheelCollider>().motorTorque = motorTorque;
        wheelRearRight.GetComponent<WheelCollider>().motorTorque = motorTorque;

        UpdateWheelPose(wheelFrontLeft, wheelFrontLeft.GetComponent<WheelCollider>());
        UpdateWheelPose(wheelFrontRight, wheelFrontRight.GetComponent<WheelCollider>());
        UpdateWheelPose(wheelRearLeft, wheelRearLeft.GetComponent<WheelCollider>());
        UpdateWheelPose(wheelRearRight, wheelRearRight.GetComponent<WheelCollider>());
    }
}
