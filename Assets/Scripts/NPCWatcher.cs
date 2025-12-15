using UnityEngine;

public class NPCWatcher : MonoBehaviour
{
    [Header("Animator control")]
    public Animator animator;
    public bool useAnimatorStates = false;

    [Header("Chill facing direction")]
    public Vector3 idleEulerAngles;

    [Header("Look behavior")]
    public float lookRotateSpeed = 5f;

    [Header("Fog Settings")]
    public float minFog = 0.02f;      // altijd actief
    public float targetFog = 0.1f;    // waarde waarop fog stijgt
    public float fogRiseSpeed = 0.05f; // snelheid van stijgen per seconde

    private bool watching = false;
    private Transform playerRef;

    private void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogDensity = minFog;
    }

    public void SetWatching(bool watch)
    {
        watching = watch;

        if (animator)
        {
            if (useAnimatorStates)
                animator.SetBool("isWatching", watch);
            else
                animator.speed = watch ? 0f : 1f;
        }
    }

    public void SetPlayerReference(Transform player)
    {
        playerRef = player;
    }

    private void Update()
    {
        if (watching)
            FacePlayerNow();
        else
            FaceIdleNow();

        UpdateFog();
    }

    private void FacePlayerNow()
    {
        if (playerRef == null) return;

        Vector3 dir = playerRef.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lookRotateSpeed * Time.deltaTime);
    }

    private void FaceIdleNow()
    {
        Quaternion targetRot = Quaternion.Euler(idleEulerAngles);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lookRotateSpeed * Time.deltaTime);
    }

    private void UpdateFog()
    {
        if (watching)
        {
            // smooth stijgen naar targetFog
            RenderSettings.fogDensity = Mathf.MoveTowards(RenderSettings.fogDensity, targetFog, fogRiseSpeed * Time.deltaTime);
        }
        else
        {
            // terug naar minFog
            RenderSettings.fogDensity = Mathf.MoveTowards(RenderSettings.fogDensity, minFog, fogRiseSpeed * Time.deltaTime);
        }
    }
}
