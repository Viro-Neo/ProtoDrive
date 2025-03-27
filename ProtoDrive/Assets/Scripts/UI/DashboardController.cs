using TMPro;
using UnityEngine;

public class DashboardController : MonoBehaviour
{
    private TextMeshProUGUI _tachometer;
    
    private void Start()
    {
        _tachometer = FindObjectOfType<Canvas>().transform.Find("RPM").GetComponent<TextMeshProUGUI>();
    }
    
    private void Update()
    {
        // Update the tachometer
        _tachometer.text = EngineController.currentRpm.ToString("F0");
    }
}
