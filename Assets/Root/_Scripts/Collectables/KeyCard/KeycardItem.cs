using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class KeycardItem : MonoBehaviour
{
    public KeycardData Data;

    private void OnTriggerEnter(Collider other)
    {
        if (Data == null)
        {
            Debug.LogWarning("Keycard " + gameObject + " is missing data");
            return;
        }

        if (other.gameObject.GetComponent<RegisterKeycards>() != null)
        {
            other.gameObject.GetComponent<RegisterKeycards>().AddKeycard(Data);
            Destroy(gameObject);
        }
    }
}
