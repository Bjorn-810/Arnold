using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGunIcon : MonoBehaviour
{
    public Image myImage;
    public GunSelector mySelector;

    private void Update()
    {
        myImage.sprite = mySelector._ActiveGun._GunIcon;
    }
}
