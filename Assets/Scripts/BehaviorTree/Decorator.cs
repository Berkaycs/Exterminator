using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : Node
{
    private Node _child;

    protected Node GetChild()
    {
        return _child;
    }

    public Decorator(Node child)
    {
        _child = child;
    }

    public override void SortPriority(ref int priorityConter)
    {
        base.SortPriority(ref priorityConter);
        _child.SortPriority(ref priorityConter);
    }

    public override Node Get()
    {
        return _child.Get();
    }
}
