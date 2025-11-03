using UnityEngine;

public class PedestrianMover : MonoBehaviour
{
    public float speed = 2f;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Each platform tag causes a turn
        if (other.CompareTag("Direction_Changer_Platform_A"))
        {
            transform.Rotate(0f, -90f, 0f);
        }
        else if (other.CompareTag("Direction_Changer_Platform_B"))
        {
            transform.Rotate(0f, 90f, 0f);
        }
        else if (other.CompareTag("Direction_Changer_Platform_C"))
        {
            transform.Rotate(0f, 180f, 0f);
        }
        else if (other.CompareTag("Direction_Changer_Platform_D"))
        {
            transform.Rotate(0f, 45f, 0f); // or whatever angle you want for D
        }
    }

    public void TurnAround()
    {
        transform.Rotate(0f, 180f, 0f);
    }
}
