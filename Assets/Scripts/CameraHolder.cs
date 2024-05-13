using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private float _cameraDistance;
    [SerializeField] private Transform _camera;

    private void Update()
    {
        _camera.position = transform.position - _camera.forward * _cameraDistance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_camera.position, transform.position);
    }
}
