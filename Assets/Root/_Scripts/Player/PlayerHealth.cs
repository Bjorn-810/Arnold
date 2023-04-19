using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [Header("Cheats")]
    [SerializeField] private bool _InfHealth;
    [SerializeField] private BarFill _healthBar;
    [SerializeField] private BarFill _shieldBar;

    private LoadNextScene _sceneLoader;

    private void Start()
    {
        _sceneLoader = FindObjectOfType<LoadNextScene>();
        _healthBar.SetMaxAmount(_MaxHealth);
        _shieldBar.SetMaxAmount(_MaxShield);
    }

    private void Update()
    {
        if (_InfHealth)
        {
            _MaxHealth = Mathf.Infinity;
            _currentHealth = Mathf.Infinity;
        }

        if (_currentHealth <= 0)
        {
            Kill();
        }
    }

    public override void AddArmor(float armorAmount)
    {
        base.AddArmor(armorAmount);

        _shieldBar.UpdateFillAmount(_currentShield);
    }

    public override void AddHealth(float healAmount)
    {
        base.AddHealth(healAmount);
        _healthBar.UpdateFillAmount(_currentHealth);
    }

    public override void DealDamage(float damageAmount)
    {
        base.DealDamage(damageAmount);

        _shieldBar.UpdateFillAmount(_currentShield);
        _healthBar.UpdateFillAmount(_currentHealth);
    }

    public override void Kill()
    {
        base.Kill();
        _shieldBar.SetMaxAmount(_currentShield);
        _healthBar.SetMaxAmount(_currentHealth);

        _sceneLoader.LoadScene("Death Screen");
    }
}
