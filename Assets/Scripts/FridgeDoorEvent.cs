using UnityEngine;

public class FridgeDoorEvent : MonoBehaviour
{
    public HouseEventManager houseEventManagerScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            houseEventManagerScript.FridgeOpened();
        }
    }
}
