using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private string _attachSlotTag;

    public abstract void Attack();

    public string GetAttachSlotTag()
    {
        return _attachSlotTag;
    }

    public GameObject Owner
    {
        get;
        private set;
    }

    public void Init(GameObject owner)
    {
        Owner = owner;
        UnEquip();
    }

    public void Equip()
    {
        gameObject.SetActive(true);
    }

    public void UnEquip() 
    { 
        gameObject.SetActive(false); 
    }

    public void DamageGameObject(GameObject objToDamage, float amount)
    {
        HealthComponent healthComponent = objToDamage.GetComponent<HealthComponent>();

        if (healthComponent != null )
        {
            healthComponent.ChangeHealth(-amount, Owner);
        }
    }
}
