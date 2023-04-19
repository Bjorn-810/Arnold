using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DashUI : MonoBehaviour
{
    private MovementPlayer _movementPlayer;
    [SerializeField] private Image _dashImage;

    void Start()
    {
        _movementPlayer = FindObjectOfType<MovementPlayer>();
    }
    
    void Update()
    {
        if(_movementPlayer._IsDashing)
        {
            _dashImage.fillAmount = 0;
        }
        else
        {
            _dashImage.fillAmount += _movementPlayer._DashCooldown;
        }
    }
}
