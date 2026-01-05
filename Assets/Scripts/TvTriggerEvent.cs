using UnityEngine;

public class TvTriggerEvent : MonoBehaviour
{
    public HouseEventManager houseEventManagerScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            houseEventManagerScript.TvTriggered();
        }
    }
}
