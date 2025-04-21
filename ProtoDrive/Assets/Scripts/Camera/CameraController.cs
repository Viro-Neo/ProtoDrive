using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform car;
	public Rigidbody rb;
	public Camera cam;
	
	const float Distance = 7f;
	public float height = 1.4f;
	public float rotationDamping = 3.0f;
	public float heightDamping = 2.0f;
	public float zoomRatio = 0.5f;
	public float defaultFOV = 60f;

	private Vector3 _rotationVector;

	void LateUpdate()
	{
		float wantedAngle = _rotationVector.y;
		float wantedHeight = car.position.y + height;
		float myAngle = transform.eulerAngles.y;
		float myHeight = transform.position.y;

		myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotationDamping * Time.deltaTime);
		myHeight = Mathf.Lerp(myHeight, wantedHeight, heightDamping * Time.deltaTime);

		// set camera rotation
		Quaternion currentRotation = Quaternion.Euler(0, myAngle, 0);

		// set camera position
		transform.position = car.position;
		transform.position -= currentRotation * Vector3.forward * Distance;

		Vector3 temp = transform.position;
		temp.y = myHeight;
		transform.position = temp;
		transform.LookAt(car);
	}

	void FixedUpdate()
	{
		Vector3 localVelocity = car.InverseTransformDirection(rb.linearVelocity);

		// if the car is moving backwards, reverse the camera
		if (localVelocity.z < -0.1f)
		{
			Vector3 temp = _rotationVector; 
			temp.y = car.eulerAngles.y + 180;
			_rotationVector = temp;
		}
		else
		{
			Vector3 temp = _rotationVector;
			temp.y = car.eulerAngles.y;
			_rotationVector = temp;
		}

		// zoom in/out depending on the car's speed
		float acc = rb.linearVelocity.magnitude;
		cam.fieldOfView = defaultFOV + acc * zoomRatio * Time.deltaTime;
	}
}
