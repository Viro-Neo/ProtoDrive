using UnityEngine;

public class FrontWheelsController : MonoBehaviour
{
    [HideInInspector] public WheelCollider leftWheel;
    [HideInInspector] public WheelCollider rightWheel;
    private Transform _leftWheelModel;
    private Transform _rightWheelModel;
    
    private const float MaxSteerAngle = 35;
    
    private Vector3 _position;
    private Quaternion _rotation;
    
    private void Start()
    {
        leftWheel = transform.Find("Left Collider").GetComponent<WheelCollider>();
        rightWheel = transform.Find("Right Collider").GetComponent<WheelCollider>();
        _leftWheelModel = transform.Find("Left Collider").GetChild(0);
        _rightWheelModel = transform.Find("Right Collider").GetChild(0);
    }
    
    private void LateUpdate()
    {
        leftWheel.GetWorldPose(out _position, out _rotation);
        _leftWheelModel.transform.position = _position;
        _leftWheelModel.transform.rotation = _rotation;
        
        rightWheel.GetWorldPose(out _position, out _rotation);
        _rightWheelModel.transform.position = _position;
        _rightWheelModel.transform.rotation = _rotation;
    }

    private void FixedUpdate()
    {
        float hInput = Input.GetAxis("Horizontal");
        
        leftWheel.steerAngle = hInput * MaxSteerAngle;
        rightWheel.steerAngle = hInput * MaxSteerAngle;
    }
}
