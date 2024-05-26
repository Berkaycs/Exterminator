using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimComponent : MonoBehaviour
{
    [SerializeField] private Transform _aimTarget;
    [SerializeField] private float _aimRange;
    [SerializeField] private LayerMask _aimMask;

    public GameObject GetAimTarget(out Vector3 aimDir)
    {
        Vector3 aimStart = _aimTarget.position;
        aimDir = GetAimDir();

        if (Physics.Raycast(aimStart, aimDir, out RaycastHit hitInfo, _aimRange, _aimMask))
        {
            return hitInfo.collider.gameObject;
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_aimTarget.position, _aimTarget.position + GetAimDir() * _aimRange);
    }

    private Vector3 GetAimDir()
    {
        Vector3 aimDir = _aimTarget.forward;
        return new Vector3(aimDir.x, 0f, aimDir.z).normalized;
    }
}
