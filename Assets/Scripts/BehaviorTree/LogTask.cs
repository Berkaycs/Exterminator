using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogTask : Node
{
    private string _message;

    public LogTask(string message)
    {
        _message = message;
    }

    protected override NodeResult Execute()
    {
        Debug.Log(_message);
        return NodeResult.Success;
    }
}
