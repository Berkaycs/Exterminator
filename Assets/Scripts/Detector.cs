using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : SenseComponent
{
    [SerializeField] private float _senseDistance = 2f;
    protected override bool IsStimuliSensable(PerceptionStimuli trigger)
    {
        return Vector3.Distance(transform.position, trigger.transform.position) <= _senseDistance;
    }

    protected override void DrawDebug()
    {
        base.DrawDebug();
        Gizmos.DrawWireSphere(transform.position + Vector3.up, _senseDistance);
    }
}
