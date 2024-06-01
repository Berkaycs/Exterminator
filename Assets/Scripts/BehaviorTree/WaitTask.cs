using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTask : Node
{
    private float _waitTime = 2f;
    private float _timeElapsed = 0f;

    public WaitTask(float waitTime)
    {
        _waitTime = waitTime;
    }

    protected override NodeResult Execute()
    {
        if (_waitTime <= 0)
        {
            return NodeResult.Success;
        }
        Debug.Log($"wait started with duration: {_waitTime}");
        _timeElapsed = 0f;
        return NodeResult.Inprogress;
    }

    protected override NodeResult Update()
    {
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed >= _waitTime)
        {
            Debug.Log("Wait finished");
            return NodeResult.Success;
        }
        //Debug.Log($"Waiting for {_timeElapsed}");
        return NodeResult.Inprogress;   
    }
}
