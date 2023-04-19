using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Shake config", menuName = "Guns/ Camera Shake Configuration", order = 4)]
public class CameraShakeConfigurationSO : ScriptableObject
{
    public float _intensity;
    public float _frequency;
    public float _shakeTime;
}
