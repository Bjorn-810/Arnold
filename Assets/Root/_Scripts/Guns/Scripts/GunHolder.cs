using System.Collections.Generic;
using UnityEngine;

public class GunHolder : MonoBehaviour
{
    [HideInInspector] public GunSO _ActiveGun;

    [SerializeField] private List<GunSO> _guns = new List<GunSO>(); // Reference to the selectable weapons
    [SerializeField] private Transform _weaponHolder;

    private void Awake()
    {
        for (int i = 0; i < _guns.Count; i++) // Instantiates new gun system to be able to switch the active gun to one if the items in the list
            _guns[i] = Instantiate(_guns[i]);

        _ActiveGun = _guns.Count > 0 ? _guns[0] : null;

        if (_ActiveGun != null)
            _ActiveGun.Spawn(_weaponHolder, this); // Spawn the gun and associate it with this controller
    }

    public void SwitchWeapon(int index)
    {
        _ActiveGun.Despawn();

        _ActiveGun = _guns[index];
        _ActiveGun.Spawn(_weaponHolder, this);
    }

    public void Shoot()
    {
        if (_ActiveGun != null)
            this._ActiveGun.Shoot(); // Call the shoot function from the GunSO scriptable object
    }
}