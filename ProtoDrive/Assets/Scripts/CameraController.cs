using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform carTransform;
	public Rigidbody carRb;

	[Header("Camera Settings")]
	[Range(3f, 7f)]
	public float followDistance = 3f; // Distance behind the car
	[Range(1f, 2f)]
	public float followHeight = 1.5f; // Height above the car
	public float followDamping = 7f; // Damping for smooth following
	[Range(2f, 7f)]
	public float rotationDamping = 2f; // Damping for smooth rotation

	private Vector3 _targetPosition;
	private Quaternion _targetRotation;
	private Vector3 _velocity = Vector3.zero;

	private void Start()
	{
		carRb.interpolation = RigidbodyInterpolation.Interpolate;
	}

	private void LateUpdate()
	{
		UpdateCameraPosition();
		UpdateCameraRotation();
		transform.LookAt(carTransform);
	}

	private void UpdateCameraPosition()
	{
		Vector3 velocity = carRb.linearVelocity;
		Vector3 direction = carTransform.forward;
		if (Vector3.Dot(carTransform.forward, velocity) < -1)
		{
			direction = -carTransform.forward;
		}

		_targetPosition = carTransform.position - direction * followDistance + Vector3.up * followHeight;
		transform.position = Vector3.Lerp(transform.position, _targetPosition, followDamping * Time.deltaTime);
	}

	private void UpdateCameraRotation()
	{
		_targetRotation = Quaternion.LookRotation(carTransform.position - transform.position, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationDamping * Time.deltaTime);
	}
}
