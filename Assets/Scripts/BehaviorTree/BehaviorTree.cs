using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    private Node _root;
    private Blackboard _blackboard = new Blackboard();
    
    public Blackboard Blackboard 
    {
        get{ return _blackboard; }
    }

    private void Start()
    {
        ConstructTree(out _root);
        SortTree();
    }

    private void SortTree()
    {
        int priorityCounter = 0;
        _root.SortPriority(ref priorityCounter);
    }

    protected abstract void ConstructTree(out Node rootNode);

    private void Update()
    {
        _root.UpdateNode();
    }

    public void AbortLowerThan(int priority)
    {
        Node currentNode = _root.Get();
        if (currentNode.GetPriority() > priority)
        {
            _root.Abort();
        }
    }
}
