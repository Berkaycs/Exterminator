using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionStimuli : MonoBehaviour
{
    void Start()
    {
        SenseComponent.RegisterStimuli(this);
    }

    private void OnDestroy()
    {
        SenseComponent.UnregisterStimuli(this);
    }
}
