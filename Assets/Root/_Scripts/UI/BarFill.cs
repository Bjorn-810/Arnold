using UnityEngine;
using UnityEngine.UI;

public class BarFill : MonoBehaviour
{
    public Image _healthSlider;
    private float _maxAmount;
    
    private void Start()
    {
        _healthSlider = GetComponent<Image>();
    }

    public void SetMaxAmount(float maxAmount)
    {
        _maxAmount = maxAmount;
    }
    
    public void UpdateFillAmount(float amount)
    {
        _healthSlider.fillAmount = amount / _maxAmount;
    }
}
