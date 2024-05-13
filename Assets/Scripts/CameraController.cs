using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _followTransform;
    [SerializeField] private float _turnSpeed = 20f;

    private void LateUpdate()
    {
        transform.position = _followTransform.position;
    }

    // rotating the camera across around Y axis
    public void AddYawInput(float amount)
    {
        transform.Rotate(Vector3.up, amount * Time.deltaTime * _turnSpeed);
    }
}
