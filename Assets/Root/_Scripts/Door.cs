using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform _middle;
    [SerializeField] private float _DetectionRange;

    [Tooltip("When left emty, the door is always unlocked")]
    [SerializeField] private KeycardData _requiredKeycard;
    private bool _locked = false;

    [Header("Components")]
    [SerializeField] private Animator _DoorAnim;
    private GameObject _player;

    private InvSlot _matchingSlot;

    private void Start()
    {
        _player = FindObjectOfType<MovementPlayer>().gameObject;
        _locked = _requiredKeycard != null; // Sets the door to locker if there is a required keycard
    }

    private void Update()
    {
        float range = Vector3.Distance(_player.transform.position, _middle.position);

        // If the player is within the range of the door.
        if (range <= _DetectionRange)
        {
            RegisterKeycards playerKeycards = _player.GetComponent<RegisterKeycards>();
            KeycardData matchingData = playerKeycards.DataList.FirstOrDefault(keycard => keycard == _requiredKeycard);

            if (matchingData != null && _locked == true)
            {
                playerKeycards.RemoveKeycard(matchingData, _matchingSlot);
                _locked = false;
            }

            if (_locked == false)
            {
                _DoorAnim.SetTrigger("Open");
            }
        }

        else // Close the door when the player is nearby. (bug prone, only closing when not in range. Should also close if the player doesnt have the correct keycard.
        {
            _DoorAnim.SetTrigger("Close");
        }
    }
}

