using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCount : MonoBehaviour
{
    //[Header("Transform")]
    //[SerializeField] private Transform _BulletSpawnPoint;
    //[SerializeField] private Transform _PlayerTransform;

    //[Header("Prefabs")]
    //[SerializeField] private GameObject _BulletPrefab;

    [Header("Ammo")]
    [SerializeField] private TextMeshProUGUI _AmmoText;
    [SerializeField] private float _MagazineSize;
    [SerializeField] private bool _InfAmmo;
    private float _currentAmmoAmount;
    private float _emptyMagazine = 0;

    private void Start()
    {
        _currentAmmoAmount = _MagazineSize;
        _AmmoText.text  = _MagazineSize.ToString() + "/24";
    }

    void Update()
    {
        if(_currentAmmoAmount >= _MagazineSize)
            _currentAmmoAmount = _MagazineSize;

        if (_InfAmmo)
        {
            _currentAmmoAmount = Mathf.Infinity;
            _MagazineSize = Mathf.Infinity;
        }


        if (_currentAmmoAmount > _emptyMagazine && Input.GetMouseButtonDown(0))
        {
            ShootGun();
        }
        else if(_currentAmmoAmount <= _emptyMagazine) 
        {
            StartCoroutine(ReloadGun());
        }

        if (_currentAmmoAmount != _MagazineSize && Input.GetKey(KeyCode.R))
            StartCoroutine(ReloadGun());

    }

    private void ShootGun()
    {
        //Instantiate(_BulletPrefab, _BulletSpawnPoint.position, _PlayerTransform.rotation);
        _currentAmmoAmount--;
        _AmmoText.text = _currentAmmoAmount.ToString() + "/24";
    }

    private IEnumerator ReloadGun()
    {
        yield return new WaitForSeconds(1.5f);
        _AmmoText.text = _MagazineSize + "/24";
        _currentAmmoAmount = _MagazineSize;
    }

    public void AddAmmoOnCollissionAmmoCrate(float amount)
    {
        _currentAmmoAmount += amount;
        _AmmoText.text = _currentAmmoAmount.ToString() + "/24";
    }
}
