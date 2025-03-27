using UnityEngine;

public class EngineController : MonoBehaviour
{
    private AnimationCurve _torqueCurve;
    private const float MaxRpm = 6000;
    private const float IdleRpm = 750;
    private const float EngineInertia = 0.15f;
    private const float AccelerationRate = 5000f;
    private const float DecelerationRate = 2000f;
    
    public static float currentRpm;
    private float _targetRpm;
    private float _currentTorque;
    private float _throttleInput;


    private void Start()
    {
        _torqueCurve = GetComponent<TorqueCurve>().torqueCurve;
        currentRpm = 750; // We assume the engine is already running
    }
    
    void Update() {
        // Get throttle input (0 to 1)
        _throttleInput = Mathf.Clamp01(Input.GetAxis("Vertical"));
        
        if (_throttleInput > 0) {
            // Increase RPM based on acceleration & torque curve
            _currentTorque = _torqueCurve.Evaluate(currentRpm);
            _targetRpm = currentRpm + (_currentTorque * _throttleInput * AccelerationRate * Time.deltaTime);
        } else {
            // Decelerate RPM when throttle is released
            _targetRpm = currentRpm - (DecelerationRate * Time.deltaTime);
        }

        // Prevent RPM from dropping below idle
        _targetRpm = Mathf.Max(_targetRpm, IdleRpm);

        // // Calculate wheel-based RPM (if in gear)
        // if (currentGear > 0) {
        //     float wheelRPM = (wheelSpeed * 60) * gearRatios[currentGear - 1] * finalDriveRatio;
        //     targetRPM = Mathf.Max(targetRPM, wheelRPM);
        // }

        // Apply smoothing using inertia
        currentRpm = Mathf.Lerp(currentRpm, _targetRpm, EngineInertia);

        // Clamp to max RPM
        currentRpm = Mathf.Clamp(currentRpm, IdleRpm, MaxRpm);
    }
}
