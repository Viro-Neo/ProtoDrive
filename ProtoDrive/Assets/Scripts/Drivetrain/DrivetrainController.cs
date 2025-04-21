using UnityEngine;

public class DrivetrainController : MonoBehaviour
{
    private enum DriveType { Rwd, Fwd, Awd }
    private const DriveType DriveTypeSelected = DriveType.Rwd;

    [Header("Transmission Settings")]
    private readonly float[] _gearRatios = {3.382f, 0, 3.321f, 1.902f, 1.308f, 1.000f, 0.759f};
    private const float FinalDriveRatio = 4.083f;
    public static int currentGear = 1;

    private float _engineTorque;
    private float _totalRatio;
    private float _totalTorque;

    private void ShiftGear()
    {
        // Shift down
        if (Input.GetKeyDown(KeyCode.G))
        {
            currentGear--;
        }
        // Shift up
        else if (Input.GetKeyDown(KeyCode.H))
        {
            currentGear++;
        }

        // Clamp gear to valid range
       currentGear = Mathf.Clamp(currentGear, 0, _gearRatios.Length - 1);
    }
    private void Update()
    {
        CalculateRatio();

        _engineTorque = EngineController.currentTorque;
        _totalTorque = _engineTorque * _totalRatio;

        ApplyDriveType();
        ShiftGear();
    }

    private void ApplyDriveType()
    {
        switch (DriveTypeSelected)
        {
            case DriveType.Rwd:
                RearWheelsController.ApplyTorque(_totalTorque / 2);
                break;
            case DriveType.Fwd:
                FrontWheelsController.ApplyTorque(_totalTorque / 2);
                break;
            case DriveType.Awd:
                FrontWheelsController.ApplyTorque(_totalTorque / 4);
                RearWheelsController.ApplyTorque(_totalTorque / 4);
                break;
            default:
                FrontWheelsController.ApplyTorque(0);
                RearWheelsController.ApplyTorque(0);
                break;
        }
    }

    private void CalculateRatio()
    {
        switch (currentGear)
        {
            case 1:
                _totalRatio = _gearRatios[currentGear];
                break;
            case 0:
                _totalRatio = _gearRatios[currentGear] * FinalDriveRatio * -1f;
                break;
            default:
                _totalRatio = _gearRatios[currentGear] * FinalDriveRatio;
                break;
        }
    }
}
