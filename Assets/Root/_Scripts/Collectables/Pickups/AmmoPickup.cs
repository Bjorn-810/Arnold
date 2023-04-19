using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private int _Amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<GunHolder>()._ActiveGun != null)    
        {
            other.gameObject.GetComponent<GunHolder>()._ActiveGun.AmmoConfig.AddAmmo(_Amount);
            Destroy(gameObject);
        }
    }
}
