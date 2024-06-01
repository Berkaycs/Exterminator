using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionComponent : MonoBehaviour
{
    [SerializeField] private SenseComponent[] _senses;
    private LinkedList<PerceptionStimuli> _currentlyPerceivedTriggers = new LinkedList<PerceptionStimuli>();

    private PerceptionStimuli _targetTrigger;

    public delegate void OnPerceptionTargetChanged(GameObject target, bool sensed);

    public event OnPerceptionTargetChanged onPerceptionTargetChanged;

    private void Start()
    {
        foreach (SenseComponent sense in _senses)
        {
            sense.onPerceptionUpdated += SenseComponent_onPerceptionUpdated;
        }
    }

    private void SenseComponent_onPerceptionUpdated(PerceptionStimuli trigger, bool successfullySensed)
    {
        var nodeFound = _currentlyPerceivedTriggers.Find(trigger);

        if (successfullySensed)
        {
            // the enemy is triggered by the player
            if (nodeFound != null)
            {
                _currentlyPerceivedTriggers.AddAfter(nodeFound, trigger);
            }
            else
            {
                _currentlyPerceivedTriggers.AddLast(trigger);
            }
        }
        else
        {
            // the enemy is not triggered by the player
            _currentlyPerceivedTriggers.Remove(nodeFound);
        }

        if (_currentlyPerceivedTriggers.Count != 0)
        {
            // there is 1 or more trigger that is happened
            PerceptionStimuli highestTrigger = _currentlyPerceivedTriggers.First.Value;
            if (_targetTrigger == null || _targetTrigger != highestTrigger)
            {
                // set the target to the prioritized one if there is no trigger
                _targetTrigger = highestTrigger;
                onPerceptionTargetChanged?.Invoke(_targetTrigger.gameObject, true);
            }
        }
        else
        {
            // there is no trigger
            if (_targetTrigger != null)
            {
                onPerceptionTargetChanged?.Invoke(_targetTrigger.gameObject, false);
                _targetTrigger = null;                
            }
        }
    }
}
