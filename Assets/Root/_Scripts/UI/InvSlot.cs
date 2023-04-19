using UnityEngine;
using UnityEngine.UI;

public class InvSlot : MonoBehaviour
{
    public KeycardData Data;
    public Image Image;

    public InvSlot(KeycardData data)
    {
        Data = data;
        Image.sprite = Data._KeyCardSprite;
    }
}
