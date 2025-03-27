using UnityEngine;

public class CarController : MonoBehaviour
{
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;

    private WheelControl[] _wheels;

    void Start()
    {
        // Find all child GameObjects that have the WheelControl script attached
        _wheels = GetComponentsInChildren<WheelControl>();
    }

    // Update is called once per frame
    void Update()
    {
        float vInput = Input.GetAxis("Vertical");

        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z;
        
        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // Check whether the user input is in the same direction
        // as the car's velocity
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in _wheels)
        {
            if (isAccelerating)
            {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.wheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                wheel.wheelCollider.brakeTorque = 0;
            }
            else
            {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.wheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.wheelCollider.motorTorque = 0;
            }
        }
    }
}
