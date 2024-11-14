using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private float distance = 3;
    private float height = 1.5f;

    private float smoothTime = 0.25f;
    Vector3 currentVelocity;

    void LateUpdate()
    {
        Vector3 newPos = player.position - player.forward * distance + Vector3.up * height;
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref currentVelocity, smoothTime);
        transform.LookAt(player);
    }
}
