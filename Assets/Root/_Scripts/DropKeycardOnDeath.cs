using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropKeycardOnDeath : MonoBehaviour
{
    public GameObject Keycard;

    private void OnDestroy()
    {
        Instantiate(Keycard, transform.position, transform.rotation);
    }
}
