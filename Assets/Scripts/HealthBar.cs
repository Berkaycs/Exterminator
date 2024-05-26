using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    private Transform _attachPoint;

    private void Update()
    {
        Vector3 attachScreenPoint = Camera.main.WorldToScreenPoint(_attachPoint.position);
        transform.position = attachScreenPoint;
    }

    public void Init(Transform attachPoint)
    {
        _attachPoint = attachPoint;
    }

    public void SetHealthSliderValue(float health, float delta, float maxHealth)
    {
        _healthSlider.value = health/maxHealth;
    }

    public void OnOwnerDead()
    {
        Destroy(gameObject);
    }
}
