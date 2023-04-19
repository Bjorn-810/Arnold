using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineCameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera _cm;
    private CinemachineBasicMultiChannelPerlin _perlinNoise;

    void Awake()
    {
        _cm = GetComponent<CinemachineVirtualCamera>();
        _perlinNoise = _cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float frequency, float shakeTime)
    {
        _perlinNoise.m_AmplitudeGain = intensity;
        _perlinNoise.m_FrequencyGain = frequency;
        StartCoroutine(WaitTime(shakeTime));
    }

    IEnumerator WaitTime(float shakeTime)
    {
        yield return new WaitForSeconds(shakeTime);
        ResetIntensity();
    }

    void ResetIntensity()
    {
        _perlinNoise.m_AmplitudeGain = 0f;
        _perlinNoise.m_FrequencyGain = 0f;
    }
}
