using UnityEngine;

public class NormalGrip : MonoBehaviour
{
    private readonly WheelFrictionCurve _normalFrontGrip = new WheelFrictionCurve
    {
        extremumSlip = 0.4f,
        extremumValue = 1.5f,  // stronger peak grip
        asymptoteSlip = 0.8f,
        asymptoteValue = 1.2f, // better grip at larger slips
        stiffness = 2.0f       // very high grip
    };

    private readonly WheelFrictionCurve _normalRearGrip = new WheelFrictionCurve
    {
        extremumSlip = 0.6f,
        extremumValue = 1.4f, // slightly less than front
        asymptoteSlip = 0.8f,
        asymptoteValue = 1.0f,
        stiffness = 1.8f
    };
}
