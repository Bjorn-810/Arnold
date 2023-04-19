using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegisterKeycards : MonoBehaviour
{
    public List<KeycardData> DataList = new List<KeycardData>();
    public List<InvSlot> SlotList = new List<InvSlot>();
    public InvSlot SlotPrefab;
    public GameObject UI;

    public void Update()
    {
        SlotList = UI.GetComponentsInChildren<InvSlot>().ToList();
    }

    public void AddKeycard(KeycardData data)
    {
        DataList.Add(data);

        InvSlot instance = SlotPrefab;
        instance.Image.sprite = data._KeyCardSprite;
        Instantiate(instance, UI.transform);
    }

    public void RemoveKeycard(KeycardData data, InvSlot slot)
    {
        Debug.Log(slot);
        Destroy(slot);
        DataList.Remove(data);
    }
}
