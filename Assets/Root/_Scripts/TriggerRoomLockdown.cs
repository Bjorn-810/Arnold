using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRoomLockdown : MonoBehaviour
{
    [Header("LockDown")]
    [HideInInspector] public bool _LockdownActive;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MovementPlayer>() != null)
        {
            _LockdownActive = true;
        }
    }
}
