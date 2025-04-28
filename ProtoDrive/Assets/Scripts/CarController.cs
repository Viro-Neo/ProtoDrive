using TMPro;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody _rb;
    private WheelCollider[] _wheelColliders;
    private Transform[] _wheelMeshes;
    private TextMeshProUGUI _speedText;

    private const float MaxMotorTorque = 1500f; // Maximum motor torque
    private const float MaxSteerAngle = 30f; // Maximum steering angle
    private const float BrakeTorque = 3000f; // Brake torque
    private const float MaxSpeed = 200f; // Maximum speed

    private readonly float[] _brakeTorqueRatio = { 0.7f, 0.3f };  // brakeTorqueRatio[0] for front wheels, brakeTorqueRatio[1] for rear wheels
    private float _currentSpeed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _wheelColliders = GetComponentsInChildren<WheelCollider>();
        _wheelMeshes = new Transform[_wheelColliders.Length];
        _speedText = GameObject.Find("Speed").GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelMeshes[i] = _wheelColliders[i].transform.GetChild(0);
        }
    }

    private void Update()
    {
        HandleInput();
        UpdateWheelMeshes();

        // Update speed display
        _currentSpeed = _rb.linearVelocity.magnitude * 3.6f; // Convert to km/h
        _speedText.text = $"{_currentSpeed:F0} ku/h";
    }

    private void HandleInput()
    {
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");
        float motor = MaxMotorTorque * vInput;
        float steer = MaxSteerAngle * hInput;

        // Apply motor torque to rear wheels
        _wheelColliders[2].motorTorque = motor;
        _wheelColliders[3].motorTorque = motor;

        // Apply steering angle to front wheels
        _wheelColliders[0].steerAngle = steer;
        _wheelColliders[1].steerAngle = steer;

        // Apply brakes
        if (vInput < 0)
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

    private void UpdateWheelMeshes()
    {
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelColliders[i].GetWorldPose(out var position, out var rotation);
            _wheelMeshes[i].position = position;
            _wheelMeshes[i].rotation = rotation;
        }
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
}
