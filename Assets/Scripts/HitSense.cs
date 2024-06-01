using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSense : SenseComponent
{
    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private float _hitMemory = 2f;

    private Dictionary<PerceptionStimuli, Coroutine> _hitRecord = new Dictionary<PerceptionStimuli, Coroutine>();

    protected override bool IsStimuliSensable(PerceptionStimuli trigger)
    {
        return _hitRecord.ContainsKey(trigger);
    }

    private void Start()
    {
        _healthComponent.onTakeDamage += HealthComponent_onTakeDamage;
    }

    private void HealthComponent_onTakeDamage(float health, float delta, float maxHealth, GameObject instigator)
    {
        PerceptionStimuli trigger = instigator.GetComponent<PerceptionStimuli>();

        if (trigger != null)
        {
            Coroutine newForgettingCoroutine = StartCoroutine(ForgetStimuli(trigger));

            if (_hitRecord.TryGetValue(trigger, out Coroutine onGoingCoroutine))
            {
                StopCoroutine(onGoingCoroutine);
                _hitRecord[trigger] = newForgettingCoroutine;
            }
            else
            {
                _hitRecord.Add(trigger, newForgettingCoroutine);
            }
        }
    }

    private IEnumerator ForgetStimuli(PerceptionStimuli trigger)
    {
        yield return new WaitForSeconds(_hitMemory);
        _hitRecord.Remove(trigger);
    }
}
