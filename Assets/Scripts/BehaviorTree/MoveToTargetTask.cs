using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTargetTask : Node
{
    private NavMeshAgent _agent;
    private string _targetKey;
    private GameObject _target;
    private float _acceptableDistance = 1f;
    private BehaviorTree _tree;

    public MoveToTargetTask(BehaviorTree tree, string targetKey, float acceptableDistance = 1f)
    {
        _targetKey = targetKey;
        _acceptableDistance = acceptableDistance;
        _tree = tree;
    }

    protected override NodeResult Execute()
    {
        Blackboard blackboard = _tree.Blackboard;

        if (blackboard == null || !blackboard.GetBlackboardData(_targetKey, out _target))
        {
            return NodeResult.Failure;
        }

        _agent = _tree.GetComponent<NavMeshAgent>();

        if (_agent == null)
        {
            return NodeResult.Failure;
        }

        if (IsTargetInAcceptableDistance())
        {
            return NodeResult.Success;
        }

        blackboard.onBlackboardValueChanged += Blackboard_onBlackboardValueChanged;

        _agent.SetDestination(_target.transform.position);
        _agent.isStopped = false;
        return NodeResult.Inprogress;
    }

    private void Blackboard_onBlackboardValueChanged(string key, object value)
    {
        if (key == _targetKey)
        {
            _target = (GameObject)value;
        }
    }

    protected override NodeResult Update()
    {
        if (_target == null)
        {
            _agent.isStopped = true;
            return NodeResult.Failure;
        }

        _agent.SetDestination(_target.transform.position);
        if (IsTargetInAcceptableDistance())
        {
            _agent.isStopped = true;
            return NodeResult.Success;
        }

        return NodeResult.Inprogress;
    }

    private bool IsTargetInAcceptableDistance()
    {
        return Vector3.Distance(_target.transform.position, _tree.transform.position) <= _acceptableDistance;
    }

    protected override void End()
    {
        _agent.isStopped = true;
        _tree.Blackboard.onBlackboardValueChanged -= Blackboard_onBlackboardValueChanged;
        base.End();
    }
}
