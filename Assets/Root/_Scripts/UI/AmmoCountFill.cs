using UnityEngine;
using UnityEngine.UI;

public class AmmoCountFill : MonoBehaviour
{
    public Image _ammoSlider;

    public GunHolder gun;

    private void Start()
    {
        _ammoSlider = GetComponent<Image>();
    }

    private void Update()
    {
        if (gun != null && gun._ActiveGun != null)
        {
            _ammoSlider.fillAmount = (float)gun._ActiveGun.AmmoConfig.CurrentClipAmmo / gun._ActiveGun.AmmoConfig.ClipSize; // Fix later to not use as much 
        }
    }
}
