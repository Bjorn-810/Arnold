using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] protected float _MaxHealth;
    [SerializeField] protected float _MaxShield;
    
    protected float _currentHealth;
    protected float _currentShield;

    private void Awake()
    {
        _currentHealth = _MaxHealth;
        _currentShield = _MaxShield;
    }
    
    public virtual void Kill() 
    {
        Destroy(gameObject);
    }

    public virtual void DealDamage(float damageAmount)
    {
        if (_currentShield != 0)
        {
            _currentShield -= damageAmount;
            _currentShield = Mathf.Clamp(_currentShield, 0, _MaxShield);
        }

        else
        {
            _currentHealth -= damageAmount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _MaxHealth);
        }
    }

    public virtual void AddHealth(float healAmount)
    {
        _currentHealth += healAmount;
    }

    public virtual void AddArmor(float armorAmount)
    {
        _currentShield += armorAmount;
    }


}
