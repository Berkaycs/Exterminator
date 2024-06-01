using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNextPatrolPointTask : Node
{
    private PatrollingComponent _patrollingComponent;
    private BehaviorTree _tree;
    private string _patrolPointKey;

    public GetNextPatrolPointTask(BehaviorTree tree, string patrolPointKey)
    {
        _patrollingComponent = tree.GetComponent<PatrollingComponent>();
        _tree = tree;
        _patrolPointKey = patrolPointKey;
    }

    protected override NodeResult Execute()
    {
        if ( _patrollingComponent != null && _patrollingComponent.GetNextPatrolPoint(out Vector3 point))
        {
            _tree.Blackboard.SetOrAddData(_patrolPointKey, point);
            return NodeResult.Success;
        }

        return NodeResult.Failure;
    }
}
