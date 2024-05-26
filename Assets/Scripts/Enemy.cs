using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private AnimancerComponent _animancer;
    [SerializeField] private ClipTransition _death;
    [SerializeField] private ClipTransition _idle;

    private void Start()
    {
        if (_healthComponent != null)
        {
            _healthComponent.onHealthEmpty += StartDeath;
            _healthComponent.onTakeDamage += TakenDamage;
        }
    }

    private void OnEnable()
    {
        _death.Events.OnEnd = OnDeathAnimationFinished;
        _animancer.Play(_idle);
    }

    private void StartDeath() 
    {
        _animancer.Play(_death);
    }

    private void OnDeathAnimationFinished()
    {
        Destroy(gameObject);
    }

    private void TakenDamage(float health, float delta, float maxHealth)
    {

    }
}
