using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo config", menuName = "Guns/ Ammo Configuration", order = 3)]
public class AmmoConfigSO : ScriptableObject
{
    public bool _cheat = false;

    //public int MaxAmmo = 1200;
    public int ClipSize = 30;

    //public int CurrentAmmo;
    public int CurrentClipAmmo;

    public float ReloadTime = 2f;

    public void AddAmmo(int amount)
    {
        if (_cheat == false)
        {
            CurrentClipAmmo += amount;
            Mathf.Clamp(CurrentClipAmmo, 0, ClipSize);
        }
    }
    
    public void Reload()
    {
        CurrentClipAmmo = ClipSize;
    }
}
