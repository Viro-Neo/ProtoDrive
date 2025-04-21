using TMPro;
using UnityEngine;

public class DashboardController : MonoBehaviour
{
    private TextMeshProUGUI _tachometer;
    private TextMeshProUGUI _gearText;
    private TextMeshProUGUI _speedometer;
    
    private void Start()
    {
        _tachometer = GameObject.Find("RPM").GetComponent<TextMeshProUGUI>();
        _gearText = GameObject.Find("Gear").GetComponent<TextMeshProUGUI>();
        _speedometer = GameObject.Find("Speed").GetComponent<TextMeshProUGUI>();
    }
    
    private void Update()
    {
        // Update the tachometer
        _tachometer.text = EngineController.currentRpm.ToString("F0");
        // Update the gear text
        switch (DrivetrainController.currentGear)
        {
            case 0:
                _gearText.text = "R";
                break;
            case 1:
                _gearText.text = "N";
                break;
            default:
                _gearText.text = (DrivetrainController.currentGear - 1).ToString();
                break;
        }
        // Update the speedometer

    }
}
