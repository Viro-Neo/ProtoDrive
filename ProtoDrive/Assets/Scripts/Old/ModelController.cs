using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    private const int CentreOfGravityOffset = -1;

    private WheelControl[] _driveWheels;
    private WheelControl[] _steerWheels;
    private Rigidbody _rb;

    ///**/// Engine specs
    private AnimationCurve _torqueCurve;
    
    private const float MaxRpm = 6000;
    private const float IdleRpm = 750;
    
    private float _currentRpm;
    private float _engineTorque;
    private TextMeshProUGUI _engineRpmText;
    
    ///**/// Gearbox specs
    private const float FinalDriveRatio = 4.083f;
    private readonly float[] _gearRatios = { 3.382f, 3.321f, 1.902f, 1.308f, 1.000f, 0.759f };
    
    private int _currentGear;
    private bool _isShifting;
    private TextMeshProUGUI _gearText;
    
    ///**/// Wheel interaction
    private const float WheelRadius = 0.326f;
    private const float MaxSteeringAngle = 35f;
    private const float BrakeTorque = 1000;
    
    private float _wheelTorque;
    private float _wheelForce;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass += Vector3.up * CentreOfGravityOffset;
        _currentGear = 1;
        _driveWheels = GetComponentsInChildren<WheelControl>().Where(wheel => wheel.motorized).ToArray();
        _steerWheels = GetComponentsInChildren<WheelControl>().Where(wheel => wheel.CompareTag("Steerable")).ToArray();
        _torqueCurve = GetComponent<TorqueCurve>().torqueCurve;
        _engineRpmText = GameObject.Find("RPM").GetComponent<TextMeshProUGUI>();
        _gearText = GameObject.Find("Gear").GetComponent<TextMeshProUGUI>();
    }
    
    private void Update()
    {
        float throttleInput = Input.GetAxis("Vertical");
        float steeringInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.G) && !_isShifting)
        {
            StartCoroutine(ShiftGear(-1));
        }
        if (Input.GetKey(KeyCode.H) && !_isShifting)
        {
            StartCoroutine(ShiftGear(1));
        }
        _gearText.text = $"Gear: {_currentGear}";
        
        if (_currentRpm > MaxRpm)
            throttleInput = 0f;
        
        // Calculate current engine RPM
        _currentRpm = Mathf.Lerp(IdleRpm, MaxRpm, throttleInput);
        _engineRpmText.text = $"RPM: {_currentRpm}";
        
        // Calculate current engine torque
        _engineTorque = _torqueCurve.Evaluate(_currentRpm) * throttleInput;
        
        // Calculate wheel torque
        _wheelTorque = _engineTorque * _gearRatios[_currentGear] * FinalDriveRatio;

        // Apply torque to the drive wheels
        _wheelForce = _wheelTorque / WheelRadius;
        
        // Apply throttle if the user input is in the same direction as the car's velocity
        if (Mathf.Sign(throttleInput) == Mathf.Sign(_rb.linearVelocity.z))
        {
            foreach (var wheel in _driveWheels)
            {
                wheel.wheelCollider.motorTorque = _wheelForce;
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
        else
        {
            // If the user input is in the opposite direction, apply brakes
            foreach (var wheel in _driveWheels)
            {
                wheel.wheelCollider.motorTorque = 0;
                wheel.wheelCollider.brakeTorque = Mathf.Abs(throttleInput) * BrakeTorque;
            }
        }
        
        foreach (var wheel in _steerWheels)
        {
            wheel.wheelCollider.steerAngle = MaxSteeringAngle * steeringInput;
        }
    }
    
    private IEnumerator ShiftGear(int direction)
    {
        _isShifting = true;
        yield return new WaitForSeconds(0.5f);
        
        _currentGear = Mathf.Clamp(_currentGear + direction, 0, _gearRatios.Length - 1);

        _isShifting = false;
    }
}
