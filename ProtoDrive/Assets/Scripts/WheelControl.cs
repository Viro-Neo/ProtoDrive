using UnityEngine;

public class WheelControl : MonoBehaviour
{
    public Transform wheelModel;

    [HideInInspector] 
    public WheelCollider WheelCollider;
    
    public bool steerable;
    public bool motorized;

    private Vector3 _position;
    private Quaternion _rotation;

    private void Start()
    {
        WheelCollider = GetComponent<WheelCollider>();
    }

    private void Update()
    {
        WheelCollider.GetWorldPose(out _position, out _rotation);
        wheelModel.transform.position = _position;
        wheelModel.transform.rotation = _rotation;
    }
}
