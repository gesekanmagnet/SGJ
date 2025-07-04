using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform mainCamera;
    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        mainCamera.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
