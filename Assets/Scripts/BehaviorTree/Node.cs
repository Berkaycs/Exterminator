using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeResult
{
    Success,
    Failure,
    Inprogress
}

public abstract class Node
{
    private bool _started = false;
    private int _priority;
    public NodeResult UpdateNode()
    {
        if (!_started)
        {
            _started = true;
            NodeResult result = Execute();

            if (result == NodeResult.Inprogress)
            {
                EndNode();
                return result;
            }
        }

        NodeResult updateResult = Update();
        if (updateResult != NodeResult.Inprogress)
        {
            EndNode();
        }
        return updateResult;
    }

    // override in child class
    protected virtual NodeResult Update()
    {
        // time based
        return NodeResult.Success;
    }

    protected virtual NodeResult Execute()
    {
        // one off thing
        return NodeResult.Success;
    }

    protected virtual void End()
    {
        // clean up
    }

    private void EndNode()
    {
        _started = false;
        End();
    }

    public void Abort()
    {
        EndNode();
    }

    public int GetPriority()
    {
        return _priority;
    }

    public virtual void SortPriority(ref int priorityConter)
    {
        _priority = priorityConter++;
    }

    public virtual Node Get()
    {
        return this;
    }
}


