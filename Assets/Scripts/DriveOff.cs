using UnityEngine;

public class DriveOff : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float turnSpeed = 120f;
    public float waypointReachDistance = 1f;

    [Header("Waypoints")]
    public Transform[] waypoints;

    private int currentIndex = 0;

    void Start()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Vector3 dir = waypoints[0].position - transform.position;
        dir.y = 0;

        transform.rotation = Quaternion.LookRotation(dir);
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        if (currentIndex >= waypoints.Length) return;

        Vector3 targetPos = waypoints[currentIndex].position;
        Vector3 dir = targetPos - transform.position;
        dir.y = 0;

        // Rotate smoothly toward waypoint
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            turnSpeed * Time.deltaTime
        );

        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Check if waypoint reached
        if (dir.magnitude <= waypointReachDistance)
        {
            currentIndex++;
        }
    }
}
