using UnityEngine;

public class FridgeDoorEvent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            Debug.Log("Fridge door opened by player.");
            // Add additional logic for when the fridge door is opened
        }
    }
}
