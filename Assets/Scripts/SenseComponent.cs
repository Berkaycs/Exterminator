using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SenseComponent : MonoBehaviour
{
    [SerializeField] private float _forgettingTime = 3f;

    public static List<PerceptionStimuli> RegisteredStimulis = new List<PerceptionStimuli>();

    public List<PerceptionStimuli> PerceivableStimulis = new List<PerceptionStimuli>();

    private Dictionary<PerceptionStimuli, Coroutine> _forgettingRoutines = new Dictionary<PerceptionStimuli, Coroutine>();

    public delegate void OnPerceptionUpdated(PerceptionStimuli trigger, bool successfullySensed);

    public event OnPerceptionUpdated onPerceptionUpdated;

    public static void RegisterStimuli(PerceptionStimuli trigger)
    {
        if (RegisteredStimulis.Contains(trigger)) return;

        RegisteredStimulis.Add(trigger);
    }

    public static void UnregisterStimuli(PerceptionStimuli trigger)
    {
        RegisteredStimulis.Remove(trigger);
    }

    protected abstract bool IsStimuliSensable(PerceptionStimuli trigger);

    private void Update()
    {
        foreach(PerceptionStimuli trigger in RegisteredStimulis)
        {
            if (IsStimuliSensable(trigger))
            {
                // the trigger is sensable
                if (!PerceivableStimulis.Contains(trigger))
                {
                    // the trigger is not in the list
                    PerceivableStimulis.Add(trigger);

                    if (_forgettingRoutines.TryGetValue(trigger, out Coroutine routine))
                    {
                        // if there's a trigger to be waited to forget
                        StopCoroutine(routine);
                        _forgettingRoutines.Remove(trigger);
                    }
                    else
                    {
                        onPerceptionUpdated?.Invoke(trigger, true);
                    }    
                }
            }
            else
            {
                // the trigger is not sensable
                if (PerceivableStimulis.Contains(trigger))
                {
                    // the trigger is in the list
                    PerceivableStimulis.Remove(trigger);
                    _forgettingRoutines.Add(trigger, StartCoroutine(ForgetStimuli(trigger)));
                }
            }
        }
    }

    IEnumerator ForgetStimuli(PerceptionStimuli trigger)
    {
        yield return new WaitForSeconds(_forgettingTime);
        _forgettingRoutines.Remove(trigger);
        onPerceptionUpdated?.Invoke(trigger, false);
    }

    protected virtual void DrawDebug()
    {

    }

    private void OnDrawGizmos()
    {
        DrawDebug();
    }
}
