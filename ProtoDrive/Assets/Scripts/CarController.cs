using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [Header("Initialization Settings")]
    private Rigidbody _rb;
    private WheelCollider[] _wheelColliders;
    private Transform[] _wheelMeshes;
    private TextMeshProUGUI _speedText;
    private TextMeshProUGUI _gearText;
    private float[] _gearSpeedLimits;
    private float _tireCircumference;

    [Header("Car Settings")]
    private const float MaxSteerAngle = 30f; // Maximum steering angle
    private const float BrakeTorque = 30000f; // Brake torque
    private const float MaxSpeed = 240f; // Maximum speed
    private const float EngineTorque = 275f; // Nm, from engine output
    private const float MaxEngineRpm = 6000f; // RPM, from engine output
    private const float FinalDriveRatio = 4.083f; // Final drive ratio
    private readonly float[] _forwardRatio = { 3.321f, 1.902f, 1.308f, 1.000f, 0.759f }; // Gear ratios
    private const float ReverseRatio = 3.382f; // Reverse gear ratio

    [Header("Update")]
    private int _currentGear;
    private bool _isReversing;

    public enum DriveType
    {
        FrontWheelDrive,
        RearWheelDrive,
        AllWheelDrive
    }

    [SerializeField] private DriveType driveType;

    private readonly float[] _brakeTorqueRatio = { 0.7f, 0.3f };  // brakeTorqueRatio[0] for front wheels, brakeTorqueRatio[1] for rear wheels
    private float _currentSpeed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _wheelColliders = GetComponentsInChildren<WheelCollider>();
        _wheelMeshes = new Transform[_wheelColliders.Length];
        _speedText = GameObject.Find("Speed").GetComponent<TextMeshProUGUI>();
        _gearText = GameObject.Find("Gear").GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelMeshes[i] = _wheelColliders[i].transform.GetChild(0);
        }

        CalculateGearSpeedLimits();
    }

    private void Update()
    {
        HandleInput();
        UpdateWheelMeshes();

        // Update speed display
        _currentSpeed = _rb.linearVelocity.magnitude * 3.6f; // Convert to km/h
        _speedText.text = $"{_currentSpeed:F0} ku/h";

        // Update gear display
        if (_isReversing)
        {
            _gearText.text = "R";
        }
        else
        {
            _gearText.text = (_currentGear + 1).ToString();
        }
    }

    private void HandleInput()
    {
        float vInput = Input.GetAxis("Vertical");
        float motor;

        HandleShifting(vInput);

        if (_isReversing)
        {
            motor = EngineTorque * ReverseRatio * Mathf.Clamp(vInput, -1f, 0f);
        }
        else
        {
            motor = EngineTorque * _forwardRatio[_currentGear] * FinalDriveRatio * Mathf.Clamp(vInput, 0f, 1f);
        }

        // Apply motor torque based on drive type
        switch (driveType)
        {
            case DriveType.FrontWheelDrive:
                _wheelColliders[0].motorTorque = motor / 2; // Distribute torque evenly
                _wheelColliders[1].motorTorque = motor / 2;
                break;
            case DriveType.RearWheelDrive:
                _wheelColliders[2].motorTorque = motor / 2; // Distribute torque evenly
                _wheelColliders[3].motorTorque = motor / 2;
                break;
            case DriveType.AllWheelDrive:
                foreach (var wheel in _wheelColliders)
                {
                    wheel.motorTorque = motor / 4; // Distribute torque evenly
                }
                break;
        }

        HandleSteering();

        // Apply brakes
        if (vInput < 0 && !_isReversing || vInput > 0 && _isReversing)
        {
            _wheelColliders[0].brakeTorque = BrakeTorque * _brakeTorqueRatio[0];
            _wheelColliders[1].brakeTorque = BrakeTorque * _brakeTorqueRatio[0];
            _wheelColliders[2].brakeTorque = BrakeTorque * _brakeTorqueRatio[1];
            _wheelColliders[3].brakeTorque = BrakeTorque * _brakeTorqueRatio[1];
        }
        else
        {
            foreach (var wheel in _wheelColliders)
            {
                wheel.brakeTorque = 0;
            }
        }

        HandleHandbrake();

        // Limit speed
        _currentSpeed = _rb.linearVelocity.magnitude * 3.6f; // Convert to km/h
        if (_currentSpeed > MaxSpeed)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * MaxSpeed / 3.6f; // Convert back to m/s
        }
    }

    private void HandleSteering()
    {
        float steer = MaxSteerAngle * Input.GetAxis("Horizontal");
        _wheelColliders[0].steerAngle = steer;
        _wheelColliders[1].steerAngle = steer;
    }

    private void HandleHandbrake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // Handbrake
            _wheelColliders[2].brakeTorque = BrakeTorque;
            _wheelColliders[3].brakeTorque = BrakeTorque;
        }
        else
        {
            _wheelColliders[2].brakeTorque = 0;
            _wheelColliders[3].brakeTorque = 0;
        }
    }

    private void HandleShifting(float vInput)
    {
        if (vInput < -0.1f && _currentSpeed < 1f && !_isReversing)
        {
            _isReversing = true;
        }
        else if (vInput > 0.1f && _currentSpeed < 1f && _isReversing)
        {
            _isReversing = false;
        }

        // Gear shifting logic (only in forward)
        if (!_isReversing)
        {
            if (_currentGear < _forwardRatio.Length - 1 && _currentSpeed > _gearSpeedLimits[_currentGear])
            {
                _currentGear++; // Shift up
            }
            else if (_currentGear > 0 && _currentSpeed < _gearSpeedLimits[_currentGear - 1] * 0.7f)
            {
                _currentGear--; // Shift down
            }
        }
    }

    // Initialization Methods
    private void UpdateWheelMeshes()
    {
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelColliders[i].GetWorldPose(out var position, out var rotation);
            _wheelMeshes[i].position = position;
            _wheelMeshes[i].rotation = rotation;
        }
    }

    private void CalculateGearSpeedLimits()
    {
        _gearSpeedLimits = new float[_forwardRatio.Length + 1];
        _tireCircumference = _wheelColliders[0].radius * Mathf.PI; // Example tire circumference in meters

        for (int i = 0; i < _forwardRatio.Length; i++)
        {
            float wheelRpmAtMaxEngine = MaxEngineRpm / (_forwardRatio[i] * FinalDriveRatio);
            float wheelSpeed = (wheelRpmAtMaxEngine * _tireCircumference) / 60f;
            _gearSpeedLimits[i] = wheelSpeed * 3.6f; // Convert to km/h
        }

        // Reverse gear speed limit
        float reverseSpeedLimit = (EngineTorque / ReverseRatio) * (_tireCircumference / 1000f) * MaxEngineRpm / 60f;
        _gearSpeedLimits[_gearSpeedLimits.Length - 1] = reverseSpeedLimit;
    }
}
