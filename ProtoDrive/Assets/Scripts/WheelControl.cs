using UnityEngine;

public class WheelControl : MonoBehaviour
{
    [HideInInspector]
    public WheelCollider wheelCollider;
    private Transform _wheelModel;
    
    public bool steerable;
    public bool motorized;

    private Vector3 _position;
    private Quaternion _rotation;

    private void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
        _wheelModel = transform.GetChild(0);
    }

    private void Update()
    {
        wheelCollider.GetWorldPose(out _position, out _rotation);
        _wheelModel.transform.position = _position;
        _wheelModel.transform.rotation = _rotation;
    }
}
