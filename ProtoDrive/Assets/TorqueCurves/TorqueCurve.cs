using UnityEngine;

public class TorqueCurve : MonoBehaviour
{
    public AnimationCurve torqueCurve;

    private void Awake()
    {
        // Define the RPM and Torque values
        float[] rpmValues = { 
            1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900, 
            2000, 2100, 2200, 2300, 2400, 2500, 2600, 2700, 2800, 2900, 
            3000, 3100, 3200, 3300, 3400, 3500, 3600, 3700, 3800, 3900, 
            4000, 4100, 4200, 4300, 4400, 4500, 4600, 4700, 4800, 4900, 
            5000, 5100, 5200, 5300, 5400, 5500, 5600, 5700, 5800, 5900, 
            6000, 6100, 6200, 6300, 6400, 6500
        };

        float[] torqueValues = { 
            60.9f, 74.9f, 88.5f, 101.6f, 114.2f, 126.3f, 138f, 149.2f, 159.9f, 170.1f, 
            179.8f, 189.1f, 197.9f, 206.2f, 214.1f, 221.5f, 228.4f, 234.8f, 240.7f, 246.2f, 
            251.2f, 255.7f, 259.8f, 263.3f, 266.4f, 269.1f, 271.2f, 272.9f, 274f, 274.8f, 
            275f, 274.9f, 274.7f, 274.2f, 273.6f, 272.8f, 271.9f, 270.8f, 269.5f, 268f, 
            266.3f, 264.5f, 262.5f, 260.3f, 258f, 255.5f, 252.8f, 249.9f, 246.9f, 243.7f, 
            240.3f, 235.4f, 228.8f, 220.6f, 210.9f, 199.6f
        };

        // Populate the AnimationCurve
        for (int i = 0; i < rpmValues.Length; i++)
        {
            torqueCurve.AddKey(rpmValues[i], torqueValues[i]);
        }
    }
}