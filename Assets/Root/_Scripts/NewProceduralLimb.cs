using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewProceduralLimb : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator MoveLeg(Vector3 targetPos, float time)
    {
        Vector3 startPos = transform.position;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
    }

    private void OnMouseDown()
    {
        StartCoroutine(MoveLeg(new Vector3(0, 0, 0), 1));
    }
}