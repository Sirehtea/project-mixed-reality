using UnityEngine;

public class teleporter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform playerTarget;
    public Transform playerPos;
    void Start()
    {
        // teleport player after 60 seconds
        Invoke("TeleportPlayer", 20f);
    }
    
    void TeleportPlayer()
    {
        playerPos.position = playerTarget.position;
        playerPos.rotation = playerTarget.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
