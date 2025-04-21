using UnityEngine;

public class FrontWheelsController : MonoBehaviour
{
    [Header("Wheel Models")]
    private static WheelCollider _leftWheel;
    private static WheelCollider _rightWheel;
    private Transform _leftWheelModel;
    private Transform _rightWheelModel;

    [Header("Steering Settings")]
    private const float MaxSteerAngle = 35;

    [Header("Wheel Settings")]
    private Vector3 _position;
    private Quaternion _rotation;
    
    private void Start()
    {
        _leftWheel = transform.Find("Left Collider").GetComponent<WheelCollider>();
        _rightWheel = transform.Find("Right Collider").GetComponent<WheelCollider>();
        _leftWheelModel = transform.Find("Left Collider").GetChild(0);
        _rightWheelModel = transform.Find("Right Collider").GetChild(0);
    }
    
    private void LateUpdate()
    {
        _leftWheel.GetWorldPose(out _position, out _rotation);
        _leftWheelModel.transform.position = _position;
        _leftWheelModel.transform.rotation = _rotation;
        
        _rightWheel.GetWorldPose(out _position, out _rotation);
        _rightWheelModel.transform.position = _position;
        _rightWheelModel.transform.rotation = _rotation;
    }

    private void FixedUpdate()
    {
        float hInput = Input.GetAxis("Horizontal");
        
        _leftWheel.steerAngle = hInput * MaxSteerAngle;
        _rightWheel.steerAngle = hInput * MaxSteerAngle;
    }

    public static void ApplyTorque(float torque)
    {
        _leftWheel.motorTorque = torque;
        _rightWheel.motorTorque = torque;
    }
}
