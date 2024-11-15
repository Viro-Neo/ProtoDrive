using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform car;
	private float distance = 7f;
	public float height = 1.4f;
	public float rotationDamping = 3.0f;
	public float heightDamping = 2.0f;
	public float zoomRatio = 0.5f;
	public float defaultFOV = 60f;

	private Vector3 rotationVector;

	void LateUpdate(){
		float wantedAngle = rotationVector.y;
		float wantedHeight = car.position.y + height;
		float myAngle = transform.eulerAngles.y;
		float myHeight = transform.position.y;

        myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotationDamping*Time.deltaTime);
        myHeight = Mathf.Lerp(myHeight, wantedHeight, heightDamping*Time.deltaTime);

        // set camera rotation
		Quaternion currentRotation = Quaternion.Euler(0, myAngle, 0);

        // set camera position
		transform.position = car.position;
		transform.position -= currentRotation * Vector3.forward*distance;

		Vector3 temp = transform.position; //temporary variable so Unity doesn't complain
		temp.y = myHeight;
		transform.position = temp;
		transform.LookAt(car);
	}

	void FixedUpdate(){
		Vector3 localVelocity = car.InverseTransformDirection(car.GetComponent<Rigidbody>().velocity);

        // if the car is moving backwards, reverse the camera
		if (localVelocity.z < -0.1f)
        {
			Vector3 temp = rotationVector; //because temporary variables seem to be removed after a closing bracket "}" we can use the same variable name multiple times.
			temp.y = car.eulerAngles.y + 180;
			rotationVector = temp;
		}
		else
        {
			Vector3 temp = rotationVector;
			temp.y = car.eulerAngles.y;
			rotationVector = temp;
		}

        // zoom in/out depending on the car's speed
		float acc = car.GetComponent<Rigidbody>().velocity.magnitude;
		GetComponent<Camera>().fieldOfView = defaultFOV + acc * zoomRatio * Time.deltaTime;  //he removed * Time.deltaTime but it works better if you leave it like this.
	}
}﻿
