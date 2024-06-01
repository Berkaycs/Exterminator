using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightSense : SenseComponent
{
    [SerializeField] private float _sightDistance = 5f;
    [SerializeField] private float _sightHalfAngle = 5f;
    [SerializeField] private float _eyeHeight = 1f;

    protected override bool IsStimuliSensable(PerceptionStimuli trigger)
    {
        // if the distance between the enemy and the player is not close enough 
        float distance = Vector3.Distance(trigger.transform.position, transform.position);
        if (distance > _sightDistance)
        {
            return false;
        }

        // if the player is not in the enemy's sight angle
        Vector3 forwardDir = transform.forward;
        Vector3 triggerDir = (trigger.transform.position - transform.position).normalized;

        if (Vector3.Angle(forwardDir, triggerDir) > _sightHalfAngle)
        {
            return false;
        }

        // if the player is not front of the enemy (there's an block)
        if (Physics.Raycast(transform.position + Vector3.up * _eyeHeight, triggerDir, out RaycastHit hitInfo, _sightDistance))
        {
            if (hitInfo.collider.gameObject != trigger.gameObject)
            {
                return false;
            }
        }

        // Trigger the enemy
        return true;
    }

    protected override void DrawDebug()
    {
        base.DrawDebug();
        Vector3 drawCenter = transform.position + Vector3.up * _eyeHeight;
        Gizmos.DrawWireSphere(drawCenter, _sightDistance);

        Vector3 leftLimitDir = Quaternion.AngleAxis(_sightHalfAngle, Vector3.up) * transform.forward;
        Vector3 rightLimitDir = Quaternion.AngleAxis(-_sightHalfAngle, Vector3.up) * transform.forward;

        Gizmos.DrawLine(drawCenter, drawCenter + leftLimitDir * _sightDistance);
        Gizmos.DrawLine(drawCenter, drawCenter + rightLimitDir * _sightDistance);
    }
}
