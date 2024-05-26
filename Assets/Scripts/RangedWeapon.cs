using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField] private AimComponent _aimComponent;
    [SerializeField] private float _damage = 5f;
    [SerializeField] private ParticleSystem _bulletVfx;

    public override void Attack()
    {
        GameObject target = _aimComponent.GetAimTarget(out Vector3 aimDir);
        DamageGameObject(target, _damage);

        _bulletVfx.transform.rotation = Quaternion.LookRotation(aimDir);
        _bulletVfx.Emit(_bulletVfx.emission.GetBurst(0).maxCount); // get first burst setting in the emission part
    }
}
