using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compositor : Node
{
    private LinkedList<Node> _children = new LinkedList<Node>();
    private LinkedListNode<Node> _currentChild = null;

    public void AddChild(Node newChild)
    {
        _children.AddLast(newChild);
    }

    protected override NodeResult Execute()
    {
        if (_children.Count == 0)
        {
            return NodeResult.Success;
        }

        _currentChild = _children.First;
        return NodeResult.Inprogress;
    }

    protected Node GetCurrentChild()
    {
        return _currentChild.Value;
    }

    protected bool Next()
    {
        if ( _currentChild != _children.Last )
        {
            _currentChild = _currentChild.Next;
            return true;
        }
        return false;
    }

    protected override void End()
    {
        if (_currentChild == null)
        {
            return;
        }
        _currentChild.Value.Abort();
        _currentChild = null;
    }

    public override void SortPriority(ref int priorityConter)
    {
        base.SortPriority(ref priorityConter);
        foreach (Node child in _children)
        {
            child.SortPriority(ref priorityConter);
        }
    }

    public override Node Get()
    {
        if (_currentChild == null)
        {
            if (_children.Count != 0)
            {
                return _children.First.Value.Get();
            }
            else
            {
                return this;
            }
        }

        return _currentChild.Value.Get();
    }
}
