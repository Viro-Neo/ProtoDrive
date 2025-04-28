using UnityEngine;

public class DefaultGrip : MonoBehaviour
{
    // Default grip values for the car's wheels
    private readonly WheelFrictionCurve _defaultForwardGrip = new WheelFrictionCurve
    {
        // The extremum slip is the slip value at which the wheel generates maximum grip.
        extremumSlip = 0.4f,

        // The extremum value is the maximum grip value at the extremum slip.
        extremumValue = 1.0f,

        // The asymptote slip is the slip value at which the grip starts to decrease.
        asymptoteSlip = 0.8f,

        // The asymptote value is the grip value at the asymptote slip.
        asymptoteValue = 0.5f,

        // The stiffness is a multiplier for the grip value.
        stiffness = 1.0f
    };

    private readonly WheelFrictionCurve _defaultSidewaysGrip = new WheelFrictionCurve
    {
        extremumSlip = 0.2f,
        extremumValue = 1.0f,
        asymptoteSlip = 0.5f,
        asymptoteValue = 0.75f,
        stiffness = 1.0f
    };
}
