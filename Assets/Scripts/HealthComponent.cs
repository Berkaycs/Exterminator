using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public delegate void OnHealthChange(float health, float delta, float maxHealth);
    public delegate void OnTakeDamage(float health, float delta, float maxHealth, GameObject instigator);
    public delegate void OnHealthEmpty();

    public event OnHealthChange onHealthChange;
    public event OnTakeDamage onTakeDamage;
    public event OnHealthEmpty onHealthEmpty;

    [SerializeField] private float _health = 100;
    [SerializeField] private float _maxHealth = 100;

    public void ChangeHealth(float amount, GameObject instigator)
    {
        if (amount == 0 || _health == 0)
        {
            return;
        }

        _health += amount;

        if (amount < 0)
        {
            onTakeDamage?.Invoke(_health, amount, _maxHealth, instigator);
        }

        onHealthChange?.Invoke(_health, amount, _maxHealth);

        if (_health <= 0)
        {
            _health = 0;
            onHealthEmpty?.Invoke();
        }

        Debug.Log($"{gameObject.name}, taking damage: {amount}, health is now: {_health} ");
    }
}
