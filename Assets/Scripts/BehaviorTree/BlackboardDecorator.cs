using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardDecorator : Decorator
{
    public enum RunCondition
    {
        KeyExists,
        KeyNotExists
    }

    public enum NotifyRule
    {
        RunConditionChange,
        KeyValueChange
    }

    public enum NotifyAbort
    {
        None,
        Self,
        Lower,
        Both
    }

    private string _key;
    private object _value;

    private BehaviorTree _tree;

    private RunCondition _runCondition;
    private NotifyRule _notifyRule;
    private NotifyAbort _notifyAbort;

    public BlackboardDecorator(BehaviorTree tree, Node child, string key, RunCondition runCondition, NotifyRule notifyRule, NotifyAbort notifyAbort) : base(child)
    {
        _tree = tree;
        _key = key;
        _runCondition = runCondition;
        _notifyRule = notifyRule;
        _notifyAbort = notifyAbort;
    }

    protected override NodeResult Execute()
    {
        Blackboard blackboard = _tree.Blackboard;
        if (blackboard == null)
        {
            return NodeResult.Failure;
        }

        blackboard.onBlackboardValueChanged -= Blackboard_onBlackboardValueChanged;
        blackboard.onBlackboardValueChanged += Blackboard_onBlackboardValueChanged;

        if (CheckRunCondition())
        {
            return NodeResult.Inprogress;
        }
        else
        {
            return NodeResult.Failure;
        }
    }

    private bool CheckRunCondition()
    {
        bool exists = _tree.Blackboard.GetBlackboardData(_key, out _value);
        switch (_runCondition)
        {
            case RunCondition.KeyExists:
                return exists;
            case RunCondition.KeyNotExists:
                return !exists;
        }

        return false;
    }

    private void Blackboard_onBlackboardValueChanged(string key, object value)
    {
        if (_key != key) return;

        if (_notifyRule == NotifyRule.RunConditionChange)
        {
            bool prevExists = _value != null;
            bool currentExists = value != null;

            if (prevExists != currentExists)
            {
                Notify(_notifyAbort);
            }
        }
        else if (_notifyRule == NotifyRule.KeyValueChange)
        {
            if (_value != value)
            {
                Notify(_notifyAbort);
            }
        }
    }

    private void Notify(NotifyAbort notifyAbort)
    {
        switch (_notifyAbort)
        {
            case NotifyAbort.None:
                break;
            case NotifyAbort.Self:
                AbortSelf();
                break;
            case NotifyAbort.Lower:
                AbortLower();
                break;
            case NotifyAbort.Both:
                AbortBoth();
                break;
        }
    }

    private void AbortBoth()
    {
        Abort();
        AbortLower();
    }

    private void AbortLower()
    {
        _tree.AbortLowerThan(GetPriority());
    }

    private void AbortSelf()
    {
        Abort();
    }

    protected override NodeResult Update()
    {
        return GetChild().UpdateNode();
    }

    protected override void End()
    {
        GetChild().Abort();
        base.End();
    }
}
