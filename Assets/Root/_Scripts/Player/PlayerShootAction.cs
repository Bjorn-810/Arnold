using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootAction : MonoBehaviour
{
    [Header("Runtime Filled")]
    private PlayerControls _playerControls;
    private GunHolder GunSelector;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        GunSelector = GetComponent<GunHolder>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
    
    void Update()
    {
        if (GunSelector._ActiveGun == null) // if there is no gun, do nothing
            return;

        if (_playerControls.Controls.ShootRight.ReadValue<float>() > 0.1f)
        {
            GunSelector.Shoot();
        }

        if (_playerControls.Controls.Reload.ReadValue<float>() > 0.1f)
        {
            GunSelector._ActiveGun.AmmoConfig.Reload();
        }
    }
}
