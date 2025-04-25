using TMPro;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody _rb;
    private WheelCollider[] _wheelColliders;
    private Transform[] _wheelMeshes;
    private TextMeshProUGUI _speedText;

    private const float _maxMotorTorque = 1500f; // Maximum motor torque
    private const float _maxSteerAngle = 30f; // Maximum steering angle
    private const float _brakeTorque = 3000f; // Brake torque
    private const float _maxSpeed = 200f; // Maximum speed

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
        _speedText.text = $"{_currentSpeed:F1} ku/h";
    }

    private void HandleInput()
    {
        float motor = _maxMotorTorque * Input.GetAxis("Vertical");
        float steer = _maxSteerAngle * Input.GetAxis("Horizontal");

        // Apply motor torque to rear wheels
        _wheelColliders[2].motorTorque = motor;
        _wheelColliders[3].motorTorque = motor;

        // Apply steering angle to front wheels
        _wheelColliders[0].steerAngle = steer;
        _wheelColliders[1].steerAngle = steer;

        // TODO: Implement footbrake and handbrake

        // Limit speed
        _currentSpeed = _rb.linearVelocity.magnitude * 3.6f; // Convert to km/h
        if (_currentSpeed > _maxSpeed)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * _maxSpeed / 3.6f; // Convert back to m/s
        }
    }

    private void UpdateWheelMeshes()
    {
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            Vector3 position;
            Quaternion rotation;
            _wheelColliders[i].GetWorldPose(out position, out rotation);
            _wheelMeshes[i].position = position;
            _wheelMeshes[i].rotation = rotation;
        }
    }
}
