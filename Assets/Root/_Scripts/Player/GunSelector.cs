using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSelector : GunHolder
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }
    }
}
