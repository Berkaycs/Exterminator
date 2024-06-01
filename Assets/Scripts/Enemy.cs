using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private AnimancerComponent _animancer;
    [SerializeField] private PerceptionComponent _perceptionComponent;
    [SerializeField] private BehaviorTree _behaviorTree;
    [SerializeField] private ClipTransition _death;
    [SerializeField] private ClipTransition _idle;

    private GameObject _target;

    private void Start()
    {
        if (_healthComponent != null)
        {
            _healthComponent.onHealthEmpty += StartDeath;
            _healthComponent.onTakeDamage += TakenDamage;
        }
        _perceptionComponent.onPerceptionTargetChanged += PerceptionComponent_onPerceptionTargetChanged;
    }

    private void PerceptionComponent_onPerceptionTargetChanged(GameObject target, bool sensed)
    {
        if (sensed)
        {
            _behaviorTree.Blackboard.SetOrAddData("Target", target);
        }
        else
        {
            _behaviorTree.Blackboard.RemoveBlackboardData("Target");
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

    private void TakenDamage(float health, float delta, float maxHealth, GameObject instigator)
    {

    }

    private void OnDrawGizmos()
    {
        if (_behaviorTree && _behaviorTree.Blackboard.GetBlackboardData("Target", out GameObject target))
        {
            Vector3 drawTargetPos = _target.transform.position + Vector3.up;
            Gizmos.DrawWireSphere(drawTargetPos, 0.7f);

            Gizmos.DrawLine(transform.position + Vector3.up, drawTargetPos);
        }
    }
}
