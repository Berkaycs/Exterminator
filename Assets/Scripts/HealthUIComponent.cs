using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIComponent : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBarToSpawn;
    [SerializeField] private Transform _healthBarAttachPoint;
    [SerializeField] private HealthComponent _healthComponent;

    private void Start()
    {
        InGameUI inGameUI = FindObjectOfType<InGameUI>();
        HealthBar newHealthBar = Instantiate(_healthBarToSpawn, inGameUI.transform);
        newHealthBar.Init(_healthBarAttachPoint);
        _healthComponent.onHealthChange += newHealthBar.SetHealthSliderValue;
        _healthComponent.onHealthEmpty += newHealthBar.OnOwnerDead;
    }
}
