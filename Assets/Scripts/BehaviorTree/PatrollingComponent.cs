using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingComponent : MonoBehaviour
{
    [SerializeField] private Transform[] _patrolPoints;
    private int _currentPatrolPointIndex = -1;

    public bool GetNextPatrolPoint(out Vector3 point)
    {
        point = Vector3.zero;
        if (_patrolPoints.Length == 0)
        {
            return false;
        }

        _currentPatrolPointIndex = (_currentPatrolPointIndex +1) % _patrolPoints.Length;  
        point = _patrolPoints[_currentPatrolPointIndex].position;
        return true;
    }
}
