using UnityEngine;
using UnityEngine.Serialization;

public class EngineController : MonoBehaviour
{
    [Header("Engine Settings")]
    private AnimationCurve _torqueCurve;
    private const float MaxRpm = 6000;
    private const float IdleRpm = 750;
    private const float EngineInertia = 0.15f;
    private const float DecelerationRate = 2000f;

    public static float currentRpm;
    public static float currentTorque;

    private float _targetRpm;
    private float _throttleInput;
    private float _accelerationRate;

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
            currentTorque = _torqueCurve.Evaluate(currentRpm);
            _accelerationRate = currentTorque / EngineInertia;
            _targetRpm = currentRpm + (currentTorque * _throttleInput * _accelerationRate * Time.deltaTime);
        } else {
            // Decelerate RPM when throttle is released
            _targetRpm = currentRpm - DecelerationRate * Time.deltaTime;
        }

        // Prevent RPM from dropping below idle
        _targetRpm = Mathf.Max(_targetRpm, IdleRpm);

        // Apply smoothing using inertia
        currentRpm = Mathf.Lerp(currentRpm, _targetRpm, EngineInertia);

        // Clamp to max RPM
        currentRpm = Mathf.Clamp(currentRpm, IdleRpm, MaxRpm);
    }
}
