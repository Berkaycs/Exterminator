using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    [SerializeField] private Weapon[] _initWeaponsPrefabs;

    [SerializeField] private AnimationOverrideSO[] _animationOverridesSO;

    [SerializeField] private Transform _defaultWeaponSlot;
    [SerializeField] private Transform[] _weaponSlots;

    private List<Weapon> _weapons;
    private Player _player;
    
    private int _currentWeaponIndex = -1;
    private int _playerIdleAnimationIndex = 2;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        InitializeWeapons();
    }

    private void InitializeWeapons()
    {
        _weapons = new List<Weapon>();
        foreach(Weapon weapon in _initWeaponsPrefabs)
        {
            Transform weaponSlot = _defaultWeaponSlot;

            foreach(Transform slot in _weaponSlots)
            {
                if (slot.gameObject.tag == weapon.GetAttachSlotTag())
                {
                    weaponSlot = slot;
                }
            }

            Weapon newWeapon = Instantiate(weapon, weaponSlot);
            newWeapon.Init(gameObject);
            _weapons.Add(newWeapon);
        }

        NextWeapon();
    }

    public void NextWeapon()
    {
        // in start we have the weapon that is 0. index
        int nextWeaponIndex = _currentWeaponIndex + 1;

        if (nextWeaponIndex >= _weapons.Count)
        {
            nextWeaponIndex = 0;
        }

        EquipWeapon(nextWeaponIndex);
    }

    public Weapon GetActiveWeapon()
    {
        return _weapons[_currentWeaponIndex];
    }

    private void EquipWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= _weapons.Count)
        {
            // index is out of bound
            return;
        }

        if (_currentWeaponIndex >= 0 && _currentWeaponIndex < _weapons.Count)
        {
            _weapons[_currentWeaponIndex].UnEquip();
            _player.Idle = _animationOverridesSO[_playerIdleAnimationIndex].DefaultIdle;
        }

        _weapons[weaponIndex].Equip();
        _player.Idle = _animationOverridesSO[weaponIndex].DefaultIdle;
        _player.Attack = _animationOverridesSO[weaponIndex].DefaultAttack;

        _currentWeaponIndex = weaponIndex;
    }
}
