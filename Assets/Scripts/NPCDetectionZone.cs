using UnityEngine;

public class NPCDetectionZone : MonoBehaviour
{
    private PedestrianMover mover; // reference to the NPC movement script

    private void Awake()
    {
        // go up to parent and get PedestrianMover so we can tell it what to do
        mover = GetComponentInParent<PedestrianMover>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // did we touch the player?
        if (other.CompareTag("Player") /*|| other.CompareTag("MainCamera")*/)
        {
            if (mover != null)
            {
                mover.TurnAround();
            }
        }
    }
}
