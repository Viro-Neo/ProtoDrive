using UnityEngine;

public class BodyController : MonoBehaviour
{
    private const float CentreOfGravityOffset = -1f;
    private Rigidbody _rb;
    
    void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
        
        // Adjust the center of mass vertically to help prevent the car from rolling
        _rb.centerOfMass += Vector3.up * CentreOfGravityOffset;
    }
}
