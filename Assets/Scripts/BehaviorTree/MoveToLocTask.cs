using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToLocTask : Node
{
    private NavMeshAgent _agent;
    private string _locKey;
    private Vector3 _loc;
    private float _acceptableDistance = 1f;
    private BehaviorTree _tree;

    public MoveToLocTask(BehaviorTree tree, string locKey, float acceptableDistance = 1f)
    {
        _tree = tree;
        _locKey = locKey;
        _acceptableDistance = acceptableDistance;
    }

    protected override NodeResult Execute()
    {
        Blackboard blackboard = _tree.Blackboard;
        if (blackboard == null || !blackboard.GetBlackboardData(_locKey, out _loc))
        {
            return NodeResult.Failure;
        }

        _agent = _tree.GetComponent<NavMeshAgent>();
        if (_agent == null)
        {
            return NodeResult.Failure;
        }

        if (IsLocInAcceptableDistance())
        {
            return NodeResult.Success;
        }

        _agent.SetDestination(_loc);
        _agent.isStopped = false;
        return NodeResult.Inprogress;
    }

    protected override NodeResult Update()
    {
        if (IsLocInAcceptableDistance())
        {
            _agent.isStopped = true;
            return NodeResult.Success;
        }

        return NodeResult.Inprogress;
    }

    private bool IsLocInAcceptableDistance()
    {
        return Vector3.Distance(_loc, _tree.transform.position ) <= _acceptableDistance;
    }

    protected override void End()
    {
        _agent.isStopped = true;
        base.End();
    }
}
